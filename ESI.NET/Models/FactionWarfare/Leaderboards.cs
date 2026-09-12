using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.FactionWarfare
{
    public class Leaderboards<T>
    {
        [JsonProperty("kills")]
        public Summary<T> Kills { get; set; }

        [JsonProperty("victory_points")]
        public Summary<T> VictoryPoints { get; set; }
    }

    public class Summary<T>
    {
        [JsonProperty("yesterday")]
        public List<T> Yesterday { get; set; } = new List<T>();

        [JsonProperty("last_week")]
        public List<T> LastWeek { get; set; } = new List<T>();

        [JsonProperty("active_total")]
        public List<T> ActiveTotal { get; set; } = new List<T>();
    }

    public class FactionTotal
    {
        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }
    }

    public class CorporationTotal
    {
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }
    }

    public class CharacterTotal
    {
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }
    }
}
