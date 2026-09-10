using Newtonsoft.Json;

namespace ESI.NET.Models.Universe
{
    public class Stargate
    {
        [JsonProperty("stargate_id")]
        public long StargateId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }

        [JsonProperty("system_id")]
        public long SystemId { get; set; }

        [JsonProperty("destination")]
        public Destination Destination { get; set; }
    }

    public class Destination
    {
        [JsonProperty("system_id")]
        public long SystemId { get; set; }

        [JsonProperty("stargate_id")]
        public long StargateId { get; set; }
    }
}
