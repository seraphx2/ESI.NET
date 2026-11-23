using Newtonsoft.Json;

namespace ESI.NET.Models.Location
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.LocationLogic.Location"/>.</summary>
    public class Location
    {
        /// <summary>Solar system the character is in.</summary>
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        /// <summary>Station the character is docked in.</summary>
        [JsonProperty("station_id")]
        public long StationId { get; set; }

        /// <summary>Structure the character is docked in.</summary>
        [JsonProperty("structure_id")]
        public long StructureId { get; set; }
    }
}
