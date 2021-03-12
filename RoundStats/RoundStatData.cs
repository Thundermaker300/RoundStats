using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;

namespace RoundStats
{
    public struct RoundStatData
    {
        public Player FirstEscapee { get; set; }
        public RoleType FirstEscapeeRole { get; set; }
        public Player FirstKill { get; set; }
        public RoleType FirstKillRole { get; set; }
        public Player LastKill { get; set; }
        public RoleType LastKillRole { get; set; }

    }
}
