using Newtonsoft.Json;

namespace ESI.NET.Models.Universe
{
    public class Structure
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("owner_id")]
        public long OwnerId { get; set; }

        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }
    }
}
