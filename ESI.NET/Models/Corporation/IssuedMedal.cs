using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Corporation
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.CorporationLogic.MedalsIssued"/>.
    /// </summary>
    public class IssuedMedal
    {
        /// <summary>ID of the character who was rewarded this medal.</summary>
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        /// <summary>When the medal was awarded.</summary>
        [JsonProperty("issued_at")]
        public DateTime IssuedAt { get; set; }

        /// <summary>ID of the character who issued the medal.</summary>
        [JsonProperty("issuer_id")]
        public long IssuerId { get; set; }

        /// <summary>ID of the medal.</summary>
        [JsonProperty("medal_id")]
        public long MedalId { get; set; }

        /// <summary>The reason given when the medal was awarded.</summary>
        [JsonProperty("reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Whether the recipient shows the medal publicly: <c>public</c> or <c>private</c>.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
