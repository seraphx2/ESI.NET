using Newtonsoft.Json;

namespace ESI.NET.Models.Sovereignty
{
    public class SystemSovereignty
    {
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        [JsonProperty("system_id")]
        public long SystemId { get; set; }
    }
}
