using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Universe
{
    public class IDLookup
    {
        [JsonProperty("agents")]
        public List<ResolvedInfo> Agents { get; set; }

        [JsonProperty("alliances")]
        public List<ResolvedInfo> Alliances { get; set; }

        [JsonProperty("characters")]
        public List<ResolvedInfo> Characters { get; set; }

        [JsonProperty("constellations")]
        public List<ResolvedInfo> Constellations { get; set; }

        [JsonProperty("corporations")]
        public List<ResolvedInfo> Corporations { get; set; }

        [JsonProperty("factions")]
        public List<ResolvedInfo> Factions { get; set; }

        [JsonProperty("inventory_types")]
        public List<ResolvedInfo> InventoryTypes { get; set; }

        [JsonProperty("regions")]
        public List<ResolvedInfo> Regions { get; set; }

        [JsonProperty("systems")]
        public List<ResolvedInfo> Systems { get; set; }

        [JsonProperty("stations")]
        public List<ResolvedInfo> Stations { get; set; }
    }
}
