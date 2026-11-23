using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.PlanetaryInteraction
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.PlanetaryInteractionLogic.Colonies"/>.
    /// </summary>
    public class Planet
    {
        /// <summary>When the colony last changed.</summary>
        [JsonProperty("last_update")]
        public DateTime LastUpdate { get; set; }

        /// <summary>Number of pins in the colony.</summary>
        [JsonProperty("num_pins")]
        public long NumberOfPins { get; set; }

        /// <summary>ID of the character who owns the colony.</summary>
        [JsonProperty("owner_id")]
        public long OwnerId { get; set; }

        /// <summary>ID of the planet.</summary>
        [JsonProperty("planet_id")]
        public long PlanetId { get; set; }

        /// <summary>
        /// The planet's type: <c>temperate</c>, <c>barren</c>, <c>oceanic</c>, <c>ice</c>,
        /// <c>gas</c>, <c>lava</c>, <c>storm</c>, or <c>plasma</c>.
        /// </summary>
        [JsonProperty("planet_type")]
        public string PlanetType { get; set; }

        /// <summary>ID of the solar system the planet is in.</summary>
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        /// <summary>The command center's upgrade level, 0 to 5.</summary>
        [JsonProperty("upgrade_level")]
        public long UpgradeLevel { get; set; }
    }
}
