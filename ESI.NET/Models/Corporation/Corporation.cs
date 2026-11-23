using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Corporation
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CorporationLogic.Information"/>.</summary>
    public class Corporation
    {
        /// <summary>
        /// Alliance the corporation is a member of (Player-owned corporation only).
        /// </summary>
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        /// <summary>ID of the corporation's CEO.</summary>
        [JsonProperty("ceo_id")]
        public long CeoId { get; set; }

        /// <summary>ID of the character who founded the corporation.</summary>
        [JsonProperty("creator_id")]
        public long CreatorId { get; set; }

        /// <summary>Corporation's founding date.</summary>
        [JsonProperty("date_founded")]
        public DateTime DateFounded { get; set; }

        /// <summary>Corporation's description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Faction the corporation is enlisted in (Player-owned corporation only).
        /// </summary>
        [JsonProperty("enlisted_faction_id")]
        public long EnlistedFactionId { get; set; }

        /// <summary>
        /// Whether members may attack each other without it counting as aggression.
        /// </summary>
        [JsonProperty("friendly_fire")]
        public string FriendlyFire { get; set; }

        /// <summary>ID of the corporation's headquarters station, if it has one.</summary>
        [JsonProperty("home_station_id")]
        public long HomeStationId { get; set; }

        /// <summary>Corporation's member count.</summary>
        [JsonProperty("member_count")]
        public long MemberCount { get; set; }

        /// <summary>Corporation's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Corporation's palette colors (Player-owned corporation only).</summary>
        [JsonProperty("palette")]
        public CorporationPalette Palette { get; set; }

        /// <summary>Number of shares the corporation has issued.</summary>
        [JsonProperty("shares")]
        public long Shares { get; set; }

        /// <summary>Corporation's state. One of <c>active</c>, <c>closed</c>.</summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>Corporation's tax rates.</summary>
        [JsonProperty("tax_rates")]
        public CorporationTaxRates TaxRates { get; set; }

        /// <summary>Corporation's short name.</summary>
        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        /// <summary>Corporation's type. One of <c>player_owned</c>, <c>npc_owned</c>.</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>Corporation's URL.</summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Whether the corporation can be at war: it can declare wars and be war-declared on.
        /// </summary>
        [JsonProperty("war_eligible")]
        public bool WarEligible { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Corporation.Corporation.TaxRates"/>.
    /// </summary>
    public class CorporationTaxRates
    {
        /// <summary>ISK tax rate (0.0% - 100.0%).</summary>
        [JsonProperty("isk")]
        public decimal Isk { get; set; }

        /// <summary>Loyalty point tax rate (0.0% - 100.0%).</summary>
        [JsonProperty("loyalty_point")]
        public decimal LoyaltyPoint { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Corporation.Corporation.Palette"/>.</summary>
    public class CorporationPalette
    {
        /// <summary>Main palette color (#rrggbb).</summary>
        [JsonProperty("main_color")]
        public string MainColor { get; set; }

        /// <summary>Secondary palette color (#rrggbb).</summary>
        [JsonProperty("secondary_color")]
        public string SecondaryColor { get; set; }

        /// <summary>Tertiary palette color (#rrggbb).</summary>
        [JsonProperty("tertiary_color")]
        public string TertiaryColor { get; set; }
    }
}
