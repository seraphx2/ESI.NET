using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Character
{
    public class Corporation
    {
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("record_id")]
        public long RecordId { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
    }
}
