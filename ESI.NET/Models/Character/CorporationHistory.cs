using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Character
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.CharacterLogic.CorporationHistory"/>.
    /// </summary>
    public class CorporationHistory
    {
        /// <summary>ID of the corporation the character joined.</summary>
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        /// <summary>True if the corporation has been deleted.</summary>
        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// An incrementing ID that can be used to canonically establish order of records in cases
        /// where dates may be ambiguous.
        /// </summary>
        [JsonProperty("record_id")]
        public long RecordId { get; set; }

        /// <summary>When the character joined that corporation.</summary>
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
    }
}
