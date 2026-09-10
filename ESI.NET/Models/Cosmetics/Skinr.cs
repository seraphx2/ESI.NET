using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Cosmetics
{
    public class ParagonCursor
    {
        [JsonProperty("after")]
        public string After { get; set; }

        [JsonProperty("before")]
        public string Before { get; set; }
    }

    /// <summary>A page of Paragon Hub SKINR listings.</summary>
    public class ParagonListingPage
    {
        [JsonProperty("listings")]
        public List<ParagonListing> Listings { get; set; } = new List<ParagonListing>();

        [JsonProperty("cursor")]
        public ParagonCursor Cursor { get; set; }
    }

    public class ParagonListing
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("skinr_id")]
        public string SkinrId { get; set; }

        [JsonProperty("seller_id")]
        public long SellerId { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        /// <summary>listed | sold_out | expired | removed</summary>
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }

        [JsonProperty("expires")]
        public DateTime Expires { get; set; }

        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }

        /// <summary>One of <c>isk</c> (number) or <c>plex</c> (integer).</summary>
        [JsonProperty("price")]
        public Dictionary<string, object> Price { get; set; }

        /// <summary>Only on the caller's own listings: one of <c>character_id</c> / <c>corporation_id</c> / <c>alliance_id</c> / <c>public</c>.</summary>
        [JsonProperty("target")]
        public Dictionary<string, object> Target { get; set; }
    }

    /// <summary>GET /cosmetics/skinr/{skinr_id} - a SKINR design.</summary>
    public class SkinrDesign
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("line")]
        public string Line { get; set; }

        [JsonProperty("creator_id")]
        public long CreatorId { get; set; }

        [JsonProperty("ship_type_id")]
        public long ShipTypeId { get; set; }

        [JsonProperty("tier")]
        public SkinrTier Tier { get; set; }

        [JsonProperty("layout")]
        public SkinrLayout Layout { get; set; }
    }

    public class SkinrTier
    {
        [JsonProperty("level")]
        public long Level { get; set; }
    }

    public class SkinrLayout
    {
        /// <summary>normal | subtract | exclusion | nested | nested_inverted</summary>
        [JsonProperty("pattern_blend_mode")]
        public string PatternBlendMode { get; set; }

        [JsonProperty("slots")]
        public List<SkinrSlot> Slots { get; set; } = new List<SkinrSlot>();
    }

    public class SkinrSlot
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>One of <c>nanocoating</c> or <c>pattern</c>.</summary>
        [JsonProperty("configuration")]
        public Dictionary<string, object> Configuration { get; set; }
    }

    /// <summary>GET /characters/{character_id}/cosmetics/skinr - the character's SKINR licenses.</summary>
    public class SkinrLicenses
    {
        [JsonProperty("licenses")]
        public List<SkinrLicense> Licenses { get; set; } = new List<SkinrLicense>();
    }

    public class SkinrLicense
    {
        [JsonProperty("skinr_id")]
        public string SkinrId { get; set; }

        [JsonProperty("activated")]
        public bool Activated { get; set; }

        [JsonProperty("unactivated")]
        public long Unactivated { get; set; }
    }

    /// <summary>GET /characters/{character_id}/cosmetics/skinr/components - the character's SKINR components.</summary>
    public class SkinrComponents
    {
        [JsonProperty("licenses")]
        public List<SkinrComponent> Licenses { get; set; } = new List<SkinrComponent>();
    }

    public class SkinrComponent
    {
        [JsonProperty("component_id")]
        public long ComponentId { get; set; }

        /// <summary>nanocoating | pattern</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>One of <c>remaining</c> (integer) or <c>unlimited</c> (boolean).</summary>
        [JsonProperty("runs")]
        public Dictionary<string, object> Runs { get; set; }
    }
}
