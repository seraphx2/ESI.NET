using Newtonsoft.Json;

namespace ESI.NET.Models.Corporation
{
    public class Facility
    {
        [JsonProperty("facility_id")]
        public long FacilityId { get; set; }

        [JsonProperty("system_id")]
        public long SystemId { get; set; }

        [JsonProperty("type_id")]
        public long TypeId { get; set; }
    }
}
