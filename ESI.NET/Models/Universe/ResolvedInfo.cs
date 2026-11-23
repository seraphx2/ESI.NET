using ESI.NET.Enumerations;
using Newtonsoft.Json;

namespace ESI.NET.Models.Universe
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.UniverseLogic.Names"/>.</summary>
    public class ResolvedInfo
    {
        /// <summary>The ID that was looked up.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>Its name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// What kind of entity the ID is: <c>alliance</c>, <c>character</c>, <c>constellation</c>,
        /// <c>corporation</c>, <c>inventory_type</c>, <c>region</c>, <c>solar_system</c>,
        /// <c>station</c>, or <c>faction</c>.
        /// </summary>
        [JsonProperty("category")]
        public ResolvedInfoCategory Category { get; set; }
    }
}
