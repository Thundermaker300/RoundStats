using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using GameCore;
using UnityEngine;
using MEC;

namespace RoundStats
{
    public class RoundData
    {
        public Player FirstEscapee;
        public string FirstEscapeeRoleColor;
        public Player FirstKiller;
        public RoleType FirstKillerRole;
        public string FirstKillerRoleColor;
        public int TotalDeaths = 0;
        public int TotalGrenades = 0;
        public int TotalMedical = 0;
        public int Total914UpgradeItems = 0;
        public int Total914UpgradePlayers = 0;
        public int TotalEscapes = 0;
        public int TotalDoorsInteracted = 0;
        public string[] statsDisplayed = new string[] { };
    }
    public class EventHandlers
    {
        Plugin plugin;
        RoundData data;
        public EventHandlers(Plugin Plugin)
        {
            plugin = Plugin;
        }

        // Server

        internal void OnRoundStarted()
        {
            data = new RoundData();
        }

        internal string GetString(string stat)
        {
            switch (stat)
            {
                case "FirstEscape":
                    if (data.FirstEscapee == null)
                        return plugin.Config.NoEscapees;
                    string playerName = data.FirstEscapee.DisplayNickname ?? data.FirstEscapee.Nickname;
                    return plugin.Config.FirstEscapee.Replace("%NAME%", $"<color={data.FirstEscapeeRoleColor}><b>{playerName}</b></color>");
                case "FirstKill":
                    if (data.FirstKiller == null)
                        return plugin.Config.NoKills;
                    string playerName2 = data.FirstKiller.DisplayNickname ?? data.FirstKiller.Nickname;
                    return plugin.Config.FirstKill.Replace("%NAME%", $"<b>{playerName2}</b>").Replace("%ROLE%", $"<color={data.FirstKillerRoleColor}>{data.FirstKillerRole}</color>");
                case "TotalDeaths":
                    return plugin.Config.TotalDeaths.Replace("%NUMBER%", $"<b>{data.TotalDeaths}</b>");
                case "TotalGrenadesThrown":
                    return plugin.Config.TotalGrenadesThrown.Replace("%NUMBER%", $"<b>{data.TotalGrenades}</b>");
                case "TotalMedicalItems":
                    return plugin.Config.TotalMedicalItems.Replace("%NUMBER%", $"<b>{data.TotalMedical}</b>");
                case "Total914Upgrades":
                    return plugin.Config.Total914Upgrades.Replace("%NUMBER%", $"<b>{data.Total914UpgradeItems}</b>").Replace("%NUMBER2%", $"<b>{data.Total914UpgradePlayers}</b>");
                case "TotalEscapes":
                    return plugin.Config.TotalEscapes.Replace("%NUMBER%", $"<b>{data.TotalEscapes}</b>");
                case "TotalDoorsInteracted":
                    return plugin.Config.TotalDoorsInteracted.Replace("%NUMBER%", $"<b>{data.TotalDoorsInteracted}</b>");
                default:
                    return string.Empty;
            }
        }

        internal void CycleStats()
        {
            var enabledStats = plugin.Config.Stats.Where(kvp => kvp.Value == true).Select((KeyValuePair<string, bool> data) => data.Key);
            int count = enabledStats.Count();
            ushort timeForEach = (ushort)(Mathf.Clamp(ConfigFile.ServerConfig.GetInt("auto_round_restart_time", 10), 5, 1000) / count*3);
            var groups = new List<List<string>> { };

            int iterator = 0;
            List<string> currentList = new List<string> { };
            foreach (string stat in enabledStats)
            {
                if (data.statsDisplayed.Contains(stat))
                    continue;
                data.statsDisplayed.Append(stat);
                currentList.Add(stat);
                iterator++;
                if (iterator > 2)
                {
                    groups.Add(currentList);
                    currentList = new List<string> { };
                    iterator = 0;
                }
            }

            if (currentList.Count > 0)
                groups.Add(currentList);

            foreach (List<string> group in groups)
            {
                StringBuilder builder = new StringBuilder();
                foreach (string stat in group)
                {
                    builder.AppendLine(GetString(stat));
                }
                Map.Broadcast(new Exiled.API.Features.Broadcast(builder.ToString(), timeForEach));
            }
        }

        internal void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (!plugin.Config.Stats.Any(kvp => kvp.Value == true))
            {
                return;
            }
            if (plugin.Config.CycleStats)
            {
                CycleStats();
            }
            else
            {
                StringBuilder builder = new StringBuilder();

                foreach (KeyValuePair<string, bool> stat in plugin.Config.Stats.Where(kvp => kvp.Value == true))
                {
                    builder.AppendLine(GetString(stat.Key));
                }

                ushort displayTime = (ushort)Mathf.Clamp(ConfigFile.ServerConfig.GetInt("auto_round_restart_time", 10), 5, 1000);

                Map.ClearBroadcasts();
                Map.Broadcast(new Exiled.API.Features.Broadcast(builder.ToString(), displayTime));
            }
        }

        // Player

        internal void OnDying(DyingEventArgs ev)
        {
            if (!Round.IsStarted)
                return;
            if (!ev.IsAllowed)
                return;
            if (ev.Killer != null && ev.Killer.Role != RoleType.None && ev.Killer.Role != RoleType.Spectator && data.FirstKiller == null)
            {
                data.FirstKiller = ev.Killer;
                data.FirstKillerRole = ev.Killer.Role;
                data.FirstKillerRoleColor = ev.Killer.RoleColor.ToHex();
            }
            data.TotalDeaths++;
        }

        internal void OnEscaping(EscapingEventArgs ev)
        {
            if (!Round.IsStarted)
                return;
            if (!ev.IsAllowed)
                return;
            if (data.FirstEscapee == null)
            {
                data.FirstEscapee = ev.Player;
                data.FirstEscapeeRoleColor = ev.Player.RoleColor.ToHex();
            }
            data.TotalEscapes++;
        }

        internal void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsHost)
                return;
            data.TotalDoorsInteracted++;
        }

        internal void OnThrowingGrenade(ThrowingGrenadeEventArgs ev)
        {
            if (ev.Type == Exiled.API.Enums.GrenadeType.Scp018)
                return;
            data.TotalGrenades++;
        }

        internal void OnUpgradingItems(UpgradingItemsEventArgs ev)
        {
            if (!Round.IsStarted)
                return;
            if (!ev.IsAllowed)
                return;
            data.Total914UpgradeItems += ev.Items.Count;
            data.Total914UpgradePlayers += ev.Players.Count;
        }

        internal void OnUsedMedicalItem(UsedMedicalItemEventArgs ev)
        {
            if (!Round.IsStarted)
                return;
            data.TotalMedical++;
        }
    }
}
