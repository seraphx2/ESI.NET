using Newtonsoft.Json;

namespace ESI.NET.Models.Corporation
{
    public class Information
    {
        [JsonProperty("alliance_id")]
        public int AllianceId { get; set; }

        [JsonProperty("ceo_id")]
        public int CeoId { get; set; }

        [JsonProperty("corporation_name")]
        public string Name { get; set; }

        [JsonProperty("member_count")]
        public int MemberCount { get; set; }

        [JsonProperty("ticker")]
        public string Ticker { get; set; }
    }
}
