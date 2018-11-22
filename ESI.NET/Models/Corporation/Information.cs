using Newtonsoft.Json;

namespace ESI.NET.Models.Corporation
{
    public class Information
    {
        [JsonProperty("alliance_id")]
        public int AllianceId { get; set; }

        [JsonProperty("ceo_id")]
        public int CeoId { get; set; }

        [JsonProperty("date_founded")]
        public string DateFounded { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("home_station_id")]
        public int HomeStationId { get; set; }

        [JsonProperty("member_count")]
        public int MemberCount { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shares")]
        public int Shares { get; set; }

        [JsonProperty("tax_rate")]
        public string TaxRate { get; set; }

        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
