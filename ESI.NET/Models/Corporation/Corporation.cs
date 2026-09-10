using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Corporation
{
    public class Corporation
    {
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        [JsonProperty("ceo_id")]
        public long CeoId { get; set; }

        [JsonProperty("creator_id")]
        public long CreatorId { get; set; }

        [JsonProperty("date_founded")]
        public DateTime DateFounded { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        [JsonProperty("home_station_id")]
        public long HomeStationId { get; set; }

        [JsonProperty("member_count")]
        public long MemberCount { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shares")]
        public long Shares { get; set; }

        [JsonProperty("tax_rate")]
        public decimal TaxRate { get; set; }

        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("war_eligible")]
        public bool WarEligible { get; set; }
    }
}
