using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Corporation
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.CorporationLogic.AllianceHistory"/>.
    /// </summary>
    public class AllianceHistory
    {
        /// <summary>
        /// ID of the alliance the corporation joined. Absent for a period when it was in no
        /// alliance.
        /// </summary>
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        /// <summary>True if the alliance has been closed.</summary>
        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// An incrementing ID that can be used to canonically establish order of records in cases
        /// where dates may be ambiguous.
        /// </summary>
        [JsonProperty("record_id")]
        public long RecordId { get; set; }

        /// <summary>When the corporation joined that alliance (or left its last one).</summary>
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
    }
}
