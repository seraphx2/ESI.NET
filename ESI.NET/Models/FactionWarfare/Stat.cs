using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.FactionWarfare
{
    public class Stat
    {
        [JsonProperty("current_rank")]
        public long CurrentRank { get; set; }

        [JsonProperty("enlisted_on")]
        public DateTime EnlistedOn { get; set; }

        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        [JsonProperty("highest_rank")]
        public long HighestRank { get; set; }

        [JsonProperty("pilots")]
        public long Pilots { get; set; }

        [JsonProperty("systems_controlled")]
        public long SystemsControlled { get; set; }

        [JsonProperty("kills")]
        public Totals Kills { get; set; }

        [JsonProperty("victory_points")]
        public Totals VictoryPoints { get; set; }
    }

    public class Totals
    {
        [JsonProperty("last_week")]
        public long LastWeek { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("yesterday")]
        public long Yesterday { get; set; }
    }
}
