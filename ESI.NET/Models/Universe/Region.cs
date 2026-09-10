using Newtonsoft.Json;

namespace ESI.NET.Models.Universe
{
    public class Region
    {
        [JsonProperty("region_id")]
        public long RegionId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("constellations")]
        public long[] Constellations { get; set; }
    }
}
