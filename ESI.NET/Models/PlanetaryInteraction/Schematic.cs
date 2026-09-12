using Newtonsoft.Json;

namespace ESI.NET.Models.PlanetaryInteraction
{
    public class Schematic
    {
        [JsonProperty("cycle_time")]
        public long CycleTime { get; set; }

        [JsonProperty("schematic_name")]
        public string Name { get; set; }
    }
}
