using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Corporation
{
    public class IssuedMedal
    {
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        [JsonProperty("issued_at")]
        public DateTime IssuedAt { get; set; }

        [JsonProperty("issuer_id")]
        public long IssuerId { get; set; }

        [JsonProperty("medal_id")]
        public long MedalId { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
