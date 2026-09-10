using Newtonsoft.Json;

namespace ESI.NET.Models.Universe
{
    public class Bloodline
    {
        [JsonProperty("bloodline_id")]
        public long Id { get; set; }

        [JsonProperty("charisma")]
        public long Charisma { get; set; }

        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("intelligence")]
        public long Intelligence { get; set; }

        [JsonProperty("memory")]
        public long Memory { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("perception")]
        public long Perception { get; set; }

        [JsonProperty("race_id")]
        public long RaceId { get; set; }

        [JsonProperty("ship_type_id")]
        public long ShipTypeId { get; set; }

        [JsonProperty("willpower")]
        public long Willpower { get; set; }

    }
}
