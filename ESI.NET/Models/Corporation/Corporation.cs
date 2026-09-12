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

        [JsonProperty("enlisted_faction_id")]
        public long EnlistedFactionId { get; set; }

        /// <summary>"legal" or "illegal"</summary>
        [JsonProperty("friendly_fire")]
        public string FriendlyFire { get; set; }

        [JsonProperty("home_station_id")]
        public long HomeStationId { get; set; }

        [JsonProperty("member_count")]
        public long MemberCount { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("palette")]
        public CorporationPalette Palette { get; set; }

        [JsonProperty("shares")]
        public long Shares { get; set; }

        /// <summary>"active" or "closed"</summary>
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("tax_rates")]
        public CorporationTaxRates TaxRates { get; set; }

        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        /// <summary>"player_owned" or "npc_owned"</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("war_eligible")]
        public bool WarEligible { get; set; }
    }

    public class CorporationTaxRates
    {
        [JsonProperty("isk")]
        public decimal Isk { get; set; }

        [JsonProperty("loyalty_point")]
        public decimal LoyaltyPoint { get; set; }
    }

    public class CorporationPalette
    {
        [JsonProperty("main_color")]
        public string MainColor { get; set; }

        [JsonProperty("secondary_color")]
        public string SecondaryColor { get; set; }

        [JsonProperty("tertiary_color")]
        public string TertiaryColor { get; set; }
    }
}
