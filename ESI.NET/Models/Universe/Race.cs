using Newtonsoft.Json;

namespace ESI.NET.Models.Universe
{
    public class Race
    {
        [JsonProperty("race_id")]
        public long RaceId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }
    }
}
