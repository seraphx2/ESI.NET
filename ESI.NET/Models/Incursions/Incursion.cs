using Newtonsoft.Json;

namespace ESI.NET.Models.Incursions
{
    public class Incursion
    {
        [JsonProperty("constellation_id")]
        public long ConstellationId { get; set; }

        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        [JsonProperty("has_boss")]
        public bool HasBoss { get; set; }

        [JsonProperty("infested_solar_systems")]
        public long[] InfestedSystems { get; set; }

        [JsonProperty("influence")]
        public double Influence { get; set; }

        [JsonProperty("staging_solar_system_id")]
        public long StagingSystemId { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
