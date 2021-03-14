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
        [Description("If set to true, all of the enabled stats will be shown 3 at a time, and will cycle through them.")]
        public bool CycleStats { get; set; } = false;
        [Description("List of stats that can be shown at the end of the round.")]
        public Dictionary<string, bool> Stats { get; set; } = new Dictionary<string, bool>()
        {
            { "FirstEscape", true },
            { "FirstKill", true },
            { "TotalDeaths", false },
            { "TotalGrenadesThrown", false },
            { "Total914Upgrades", false },
            { "TotalEscapes", false },
            { "TotalDoorsInteracted", false },
        };
        [Description("Translation")]
        public string FirstEscapee { get; set; } = "%NAME% was the first player to escape!";
        public string NoEscapees { get; set; } = "Nobody escaped this round!";
        public string FirstKill { get; set; } = "%NAME% (%ROLE%) made the first kill this round.";
        public string NoKills { get; set; } = "Nobody was killed this round!";
        public string TotalDeaths { get; set; } = "A total of %NUMBER% players died.";
        public string TotalGrenadesThrown { get; set; } = "%NUMBER% grenades were thrown.";
        public string Total914Upgrades { get; set; } = "%NUMBER% items and %NUMBER2% players were refined in SCP-914.";
        public string TotalEscapes { get; set; } = "%NUMBER% players escaped.";
        public string TotalDoorsInteracted { get; set; } = "%NUMBER% doors were interacted with.";

    }
}
