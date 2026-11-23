using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Cosmetics
{
    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Cosmetics.ParagonListingPage.Cursor"/>.
    /// </summary>
    public class ParagonCursor
    {
        /// <summary>
        /// Cursor to use as 'after' in your next request, to continue walking forwards in time.
        /// </summary>
        [JsonProperty("after")]
        public string After { get; set; }

        /// <summary>
        /// Cursor to use as 'before' in your next request, to continue walking backwards in time.
        /// </summary>
        [JsonProperty("before")]
        public string Before { get; set; }
    }

    /// <summary>A page of Paragon Hub SKINR listings.</summary>
    public class ParagonListingPage
    {
        /// <summary>Page of public SKINR listings.</summary>
        [JsonProperty("listings")]
        public List<ParagonListing> Listings { get; set; } = new List<ParagonListing>();

        /// <summary>
        /// Cursors for paging: pass <c>Before</c> or <c>After</c> to the next call's <c>before</c>
        /// / <c>after</c> parameter.
        /// </summary>
        [JsonProperty("cursor")]
        public ParagonCursor Cursor { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Cosmetics.ParagonListingPage.Listings"/>.
    /// </summary>
    public class ParagonListing
    {
        /// <summary>Unique identifier of the listing.</summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>SKINR license identifier.</summary>
        [JsonProperty("skinr_id")]
        public string SkinrId { get; set; }

        /// <summary>Character that posted the listing.</summary>
        [JsonProperty("seller_id")]
        public long SellerId { get; set; }

        /// <summary>How many licenses remain in this listing.</summary>
        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        /// <summary>
        /// Lifecycle state of the listing. One of <c>listed</c>, <c>sold_out</c>, <c>expired</c>,
        /// <c>removed</c>.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>When the listing was created.</summary>
        [JsonProperty("created")]
        public DateTime Created { get; set; }

        /// <summary>When the listing expires.</summary>
        [JsonProperty("expires")]
        public DateTime Expires { get; set; }

        /// <summary>When this listing was last retrieved from the game.</summary>
        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Asking price of the listing: either <c>isk</c> (a number) or <c>plex</c> (an integer).
        /// </summary>
        [JsonProperty("price")]
        public Dictionary<string, object> Price { get; set; }

        /// <summary>
        /// Who the listing is visible to. Only present on the caller's own listings: one of
        /// <c>character_id</c>, <c>corporation_id</c>, <c>alliance_id</c>, or <c>public</c>.
        /// </summary>
        [JsonProperty("target")]
        public Dictionary<string, object> Target { get; set; }
    }

    /// <summary>GET /cosmetics/skinr/{skinr_id} - a SKINR design.</summary>
    public class SkinrDesign
    {
        /// <summary>SKINR ID.</summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>SKINR name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>SKINR line name.</summary>
        [JsonProperty("line")]
        public string Line { get; set; }

        /// <summary>SKINR creator character ID.</summary>
        [JsonProperty("creator_id")]
        public long CreatorId { get; set; }

        /// <summary>ID of the ship hull.</summary>
        [JsonProperty("ship_type_id")]
        public long ShipTypeId { get; set; }

        /// <summary>SKINR tier.</summary>
        [JsonProperty("tier")]
        public SkinrTier Tier { get; set; }

        /// <summary>SKINR layout.</summary>
        [JsonProperty("layout")]
        public SkinrLayout Layout { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Cosmetics.SkinrDesign.Tier"/>.</summary>
    public class SkinrTier
    {
        /// <summary>SKINR tier level.</summary>
        [JsonProperty("level")]
        public long Level { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Cosmetics.SkinrDesign.Layout"/>.</summary>
    public class SkinrLayout
    {
        /// <summary>
        /// Blend mode to use when compositing patterns. One of <c>normal</c>, <c>subtract</c>,
        /// <c>exclusion</c>, <c>nested</c>, <c>nested_inverted</c>.
        /// </summary>
        [JsonProperty("pattern_blend_mode")]
        public string PatternBlendMode { get; set; }

        /// <summary>Cosmetic slots in the layout.</summary>
        [JsonProperty("slots")]
        public List<SkinrSlot> Slots { get; set; } = new List<SkinrSlot>();
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Cosmetics.SkinrLayout.Slots"/>.</summary>
    public class SkinrSlot
    {
        /// <summary>ID of cosmetic slot (see SDE for details).</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Configuration of the cosmetic slot: either <c>nanocoating</c> or <c>pattern</c>.
        /// </summary>
        [JsonProperty("configuration")]
        public Dictionary<string, object> Configuration { get; set; }
    }

    /// <summary>GET /characters/{character_id}/cosmetics/skinr - the character's SKINR licenses.</summary>
    public class SkinrLicenses
    {
        /// <summary>SKINR licenses the character owns.</summary>
        [JsonProperty("licenses")]
        public List<SkinrLicense> Licenses { get; set; } = new List<SkinrLicense>();
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Cosmetics.SkinrLicenses.Licenses"/>.
    /// </summary>
    public class SkinrLicense
    {
        /// <summary>SKINR identifier.</summary>
        [JsonProperty("skinr_id")]
        public string SkinrId { get; set; }

        /// <summary>Whether this SKINR is activated.</summary>
        [JsonProperty("activated")]
        public bool Activated { get; set; }

        /// <summary>Number of unactivated copies of this SKINR.</summary>
        [JsonProperty("unactivated")]
        public long Unactivated { get; set; }
    }

    /// <summary>GET /characters/{character_id}/cosmetics/skinr/components - the character's SKINR components.</summary>
    public class SkinrComponents
    {
        /// <summary>SKINR component licenses the character owns.</summary>
        [JsonProperty("licenses")]
        public List<SkinrComponent> Licenses { get; set; } = new List<SkinrComponent>();
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Cosmetics.SkinrComponents.Licenses"/>.
    /// </summary>
    public class SkinrComponent
    {
        /// <summary>ID of the component (see SDE for details).</summary>
        [JsonProperty("component_id")]
        public long ComponentId { get; set; }

        /// <summary>Kind of component. One of <c>nanocoating</c>, <c>pattern</c>.</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Remaining uses of this license: either <c>remaining</c> (an integer) or <c>unlimited</c>
        /// (a boolean).
        /// </summary>
        [JsonProperty("runs")]
        public Dictionary<string, object> Runs { get; set; }
    }
}
