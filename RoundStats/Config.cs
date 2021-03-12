using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoundStats
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        [Description("If set to true, random stats from the enabled stats list will be chosen. If set to false, all enabled stats will be displayed.")]
        public bool PickRandom { get; set; } = false;
        [Description("If pick_random is set to true, this determines how many stats can be shown at once.")]
        public int RandomMax = 3;
        [Description("List of stats that can be shown at the end of the round.")]
        public bool FirstEscape { get; set; } = true;
        public bool FirstKill { get; set; } = true;
        public bool LastKill { get; set; } = false;
        public bool MostKills { get; set; } = true;
        public bool TotalGrenadesThrown { get; set; } = false;
        public bool Total914Upgrades { get; set; } = false;
        [Description("Translation")]
        public string TFirstEscape { get; set; } = "%NAME% was the first player to escape!";

    }
}
