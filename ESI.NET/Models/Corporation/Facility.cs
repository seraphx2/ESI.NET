using Newtonsoft.Json;

namespace ESI.NET.Models.Corporation
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CorporationLogic.Facilities"/>.</summary>
    public class Facility
    {
        /// <summary>ID of the station or structure that is the industry facility.</summary>
        [JsonProperty("facility_id")]
        public long FacilityId { get; set; }

        /// <summary>ID of the solar system the facility is in.</summary>
        [JsonProperty("system_id")]
        public long SystemId { get; set; }

        /// <summary>Type ID of the facility.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }
    }
}
