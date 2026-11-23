using Newtonsoft.Json;

namespace ESI.NET.Models.Industry
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.IndustryLogic.Facilities"/>.</summary>
    public class Facility
    {
        /// <summary>ID of the facility.</summary>
        [JsonProperty("facility_id")]
        public long FacilityId { get; set; }

        /// <summary>Tax imposed by the facility.</summary>
        [JsonProperty("tax")]
        public decimal Tax { get; set; }

        /// <summary>ID of the corporation that owns the facility.</summary>
        [JsonProperty("owner_id")]
        public long OwnerId { get; set; }

        /// <summary>Type ID of the facility.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>Solar system ID where the facility is.</summary>
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        /// <summary>Region ID where the facility is.</summary>
        [JsonProperty("region_id")]
        public long RegionId { get; set; }
    }
}
