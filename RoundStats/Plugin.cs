using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Exiled.API.Features;
using Exiled.Events.Handlers;

using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace RoundStats
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Singleton;
        private static EventHandlers _Handler;

        public override void OnEnabled()
        {
            Singleton = this;
            _Handler = new EventHandlers(this);

            Server.WaitingForPlayers += _Handler.OnWaitingForPlayers;
            Server.RoundEnded += _Handler.OnRoundEnded;

            Player.Dying += _Handler.OnDying;
            Player.Escaping += _Handler.OnEscaping;
            Player.InteractingDoor += _Handler.OnInteractingDoor;
            Player.ThrowingGrenade += _Handler.OnThrowingGrenade;
            Exiled.Events.Handlers.Scp914.UpgradingItems += _Handler.OnUpgradingItems;
            Player.MedicalItemUsed += _Handler.OnUsedMedicalItem;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Server.RoundStarted -= _Handler.OnWaitingForPlayers;
            Server.RoundEnded -= _Handler.OnRoundEnded;

            Player.Dying -= _Handler.OnDying;
            Player.Escaping -= _Handler.OnEscaping;
            Player.InteractingDoor -= _Handler.OnInteractingDoor;
            Player.ThrowingGrenade -= _Handler.OnThrowingGrenade;
            Exiled.Events.Handlers.Scp914.UpgradingItems -= _Handler.OnUpgradingItems;
            Player.MedicalItemUsed -= _Handler.OnUsedMedicalItem;

            _Handler = null;
            Singleton = null;
            base.OnDisabled();
        }

        public override string Name => "RoundStats";
        public override string Author => "Thunder";
        public override Version Version => new Version(0, 1, 0);
        public override Version RequiredExiledVersion => new Version(2, 8, 0);
    }
}
