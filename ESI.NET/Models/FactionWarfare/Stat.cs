using Newtonsoft.Json;

namespace ESI.NET.Models.FactionWarfare
{
    public class Stat
    {
        [JsonProperty("faction_id")]
        public int FactionId { get; set; }

        [JsonProperty("enlisted_on")]
        public string EnlistedOn { get; set; }

        [JsonProperty("pilots")]
        public int Pilots { get; set; }

        [JsonProperty("systems_controlled")]
        public int SystemsControlled { get; set; }

        [JsonProperty("current_rank")]
        public int CurrentRank { get; set; }

        [JsonProperty("highest_rank")]
        public int HighestRank { get; set; }

        [JsonProperty("kills")]
        public Totals Kills { get; set; }

        [JsonProperty("victory_points")]
        public Totals VictoryPoints { get; set; }
    }

    public class Totals
    {
        [JsonProperty("yesterday")]
        public int Yesterday { get; set; }

        [JsonProperty("last_week")]
        public int LastWeek { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
