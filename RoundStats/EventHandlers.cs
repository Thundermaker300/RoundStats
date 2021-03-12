using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using GameCore;
using UnityEngine;

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
        public int Total914UpgradeItems = 0;
        public int Total914UpgradePlayers = 0;
        public int TotalEscapes = 0;
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
                case "Total914Upgrades":
                    return plugin.Config.Total914Upgrades.Replace("%NUMBER%", $"<b>{data.Total914UpgradeItems}</b>").Replace("%NUMBER2%", $"<b>{data.Total914UpgradePlayers}</b>");
                case "TotalEscapes":
                    return plugin.Config.TotalGrenadesThrown.Replace("%NUMBER%", $"<b>{data.TotalEscapes}</b>");

                default:
                    return string.Empty;
            }
        }

        internal void OnRoundEnded(RoundEndedEventArgs ev)
        {
            StringBuilder builder = new StringBuilder();
            if (plugin.Config.PickRandom)
            {
                
            }
            else
            {
                foreach (KeyValuePair<string, bool> stat in plugin.Config.Stats.Where(kvp => kvp.Value == true))
                {
                    builder.AppendLine(GetString(stat.Key));
                }
            }

            ushort displayTime = (ushort)Mathf.Clamp(ConfigFile.ServerConfig.GetInt("auto_round_restart_time", 10), 5, 1000);

            Map.ClearBroadcasts();
            Map.Broadcast(new Exiled.API.Features.Broadcast(builder.ToString(), displayTime));
        }

        // Player

        internal void OnDying(DyingEventArgs ev)
        {
            if (!Round.IsStarted)
                return;
            if (!ev.IsAllowed)
                return;
            if (ev.Killer != null && data.FirstKiller == null)
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
    }
}
