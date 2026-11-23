using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Corporation
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.CorporationLogic.MemberTracking"/>.
    /// </summary>
    public class MemberInfo
    {
        /// <summary>ID of the member's base station.</summary>
        [JsonProperty("base_id")]
        public long BaseId { get; set; }

        /// <summary>ID of the member.</summary>
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        /// <summary>Where the member is now: a station, a structure, or a solar system.</summary>
        [JsonProperty("location_id")]
        public long LocationId { get; set; }

        /// <summary>When the member last logged off.</summary>
        [JsonProperty("logoff_date")]
        public DateTime LogoffDate { get; set; }

        /// <summary>When the member last logged on.</summary>
        [JsonProperty("logon_date")]
        public DateTime LogonDate { get; set; }

        /// <summary>Type ID of the ship the member is in.</summary>
        [JsonProperty("ship_type_id")]
        public long ShipTypeId { get; set; }

        /// <summary>When the member joined the corporation.</summary>
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
    }
}
