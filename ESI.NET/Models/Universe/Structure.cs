using Newtonsoft.Json;

namespace ESI.NET.Models.Universe
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.UniverseLogic.Structure"/>.</summary>
    public class Structure
    {
        /// <summary>The full name of the structure.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The ID of the corporation who owns this particular structure.</summary>
        [JsonProperty("owner_id")]
        public long OwnerId { get; set; }

        /// <summary>ID of the solar system the structure is in.</summary>
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        /// <summary>Type ID of the structure (Astrahus, Fortizar, ...).</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>
        /// Coordinates of the structure in Cartesian space relative to the Sun, in metres.
        /// </summary>
        [JsonProperty("position")]
        public Position Position { get; set; }
    }
}
