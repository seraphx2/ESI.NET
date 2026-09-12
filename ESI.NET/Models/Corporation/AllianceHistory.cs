using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Corporation
{
    public class AllianceHistory
    {
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("record_id")]
        public long RecordId { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
    }
}
