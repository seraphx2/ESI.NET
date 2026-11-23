using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.FactionWarfare
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.FactionWarfareLogic.Stats"/>,
    /// <see cref="ESI.NET.Logic.FactionWarfareLogic.StatsForCharacter"/>, and
    /// <see cref="ESI.NET.Logic.FactionWarfareLogic.StatsForCorporation"/>.
    /// </summary>
    public class Stat
    {
        /// <summary>The given character's current faction rank.</summary>
        [JsonProperty("current_rank")]
        public long CurrentRank { get; set; }

        /// <summary>
        /// The enlistment date of the given corporation into faction warfare. Will not be included
        /// if corporation is not enlisted in faction warfare.
        /// </summary>
        [JsonProperty("enlisted_on")]
        public DateTime EnlistedOn { get; set; }

        /// <summary>
        /// The faction the given corporation is enlisted to fight for. Will not be included if
        /// corporation is not enlisted in faction warfare.
        /// </summary>
        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        /// <summary>The given character's highest faction rank achieved.</summary>
        [JsonProperty("highest_rank")]
        public long HighestRank { get; set; }

        /// <summary>How many pilots fight for the given faction.</summary>
        [JsonProperty("pilots")]
        public long Pilots { get; set; }

        /// <summary>The number of solar systems controlled by the given faction.</summary>
        [JsonProperty("systems_controlled")]
        public long SystemsControlled { get; set; }

        /// <summary>Summary of kills against an enemy faction for the given faction.</summary>
        [JsonProperty("kills")]
        public Totals Kills { get; set; }

        /// <summary>Summary of victory points gained for the given faction.</summary>
        [JsonProperty("victory_points")]
        public Totals VictoryPoints { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.FactionWarfare.Stat.Kills"/> and
    /// <see cref="ESI.NET.Models.FactionWarfare.Stat.VictoryPoints"/>.
    /// </summary>
    public class Totals
    {
        /// <summary>Last week's total number of kills against enemy factions.</summary>
        [JsonProperty("last_week")]
        public long LastWeek { get; set; }

        /// <summary>
        /// Total number of kills against enemy factions since faction warfare began.
        /// </summary>
        [JsonProperty("total")]
        public long Total { get; set; }

        /// <summary>Yesterday's total number of kills against enemy factions.</summary>
        [JsonProperty("yesterday")]
        public long Yesterday { get; set; }
    }
}
