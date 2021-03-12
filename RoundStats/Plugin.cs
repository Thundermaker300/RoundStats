using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Exiled.API.Features;
using Exiled.API.Interfaces;

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
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            _Handler = null;
            Singleton = null;
            base.OnDisabled();
        }

        public override string Name => "MVP";
        public override string Author => "Thunder";
        public override Version Version => new Version(0, 0, 0);
        public override Version RequiredExiledVersion => new Version(2, 8, 0);
    }
}
