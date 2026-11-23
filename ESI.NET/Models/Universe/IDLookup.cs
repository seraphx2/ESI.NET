using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Universe
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.UniverseLogic.IDs"/>.</summary>
    public class IDLookup
    {
        /// <summary>Agents whose names matched, with their IDs.</summary>
        [JsonProperty("agents")]
        public List<ResolvedInfo> Agents { get; set; } = new List<ResolvedInfo>();

        /// <summary>Alliances whose names matched, with their IDs.</summary>
        [JsonProperty("alliances")]
        public List<ResolvedInfo> Alliances { get; set; } = new List<ResolvedInfo>();

        /// <summary>Characters whose names matched, with their IDs.</summary>
        [JsonProperty("characters")]
        public List<ResolvedInfo> Characters { get; set; } = new List<ResolvedInfo>();

        /// <summary>Constellations whose names matched, with their IDs.</summary>
        [JsonProperty("constellations")]
        public List<ResolvedInfo> Constellations { get; set; } = new List<ResolvedInfo>();

        /// <summary>Corporations whose names matched, with their IDs.</summary>
        [JsonProperty("corporations")]
        public List<ResolvedInfo> Corporations { get; set; } = new List<ResolvedInfo>();

        /// <summary>Factions whose names matched, with their IDs.</summary>
        [JsonProperty("factions")]
        public List<ResolvedInfo> Factions { get; set; } = new List<ResolvedInfo>();

        /// <summary>Item types whose names matched, with their type IDs.</summary>
        [JsonProperty("inventory_types")]
        public List<ResolvedInfo> InventoryTypes { get; set; } = new List<ResolvedInfo>();

        /// <summary>Regions whose names matched, with their IDs.</summary>
        [JsonProperty("regions")]
        public List<ResolvedInfo> Regions { get; set; } = new List<ResolvedInfo>();

        /// <summary>Solar systems whose names matched, with their IDs.</summary>
        [JsonProperty("systems")]
        public List<ResolvedInfo> Systems { get; set; } = new List<ResolvedInfo>();

        /// <summary>Stations whose names matched, with their IDs.</summary>
        [JsonProperty("stations")]
        public List<ResolvedInfo> Stations { get; set; } = new List<ResolvedInfo>();
    }
}
