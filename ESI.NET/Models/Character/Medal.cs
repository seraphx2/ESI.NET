using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Character
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CharacterLogic.Medals"/>.</summary>
    public class Medal
    {
        /// <summary>ID of the corporation that awarded the medal.</summary>
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        /// <summary>When the medal was awarded.</summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>The medal's description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>The layers the medal is drawn from.</summary>
        [JsonProperty("graphics")]
        public List<GraphicLayer> Graphics { get; set; } = new List<GraphicLayer>();

        /// <summary>ID of the character who awarded the medal.</summary>
        [JsonProperty("issuer_id")]
        public long IssuerId { get; set; }

        /// <summary>ID of the medal.</summary>
        [JsonProperty("medal_id")]
        public long MedalId { get; set; }

        /// <summary>The reason given when the medal was awarded.</summary>
        [JsonProperty("reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Whether the medal shows on the character's public profile: <c>public</c> or
        /// <c>private</c>.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>The medal's name.</summary>
        [JsonProperty("title")]
        public string Title { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Character.Medal.Graphics"/>.</summary>
    public class GraphicLayer
    {
        /// <summary>Colour of this layer, as a packed ARGB integer.</summary>
        [JsonProperty("color")]
        public long Color { get; set; }

        /// <summary>Name of the graphic drawn on this layer.</summary>
        [JsonProperty("graphic")]
        public string Graphic { get; set; }

        /// <summary>Draw order of this layer; higher layers are drawn on top.</summary>
        [JsonProperty("layer")]
        public long Layer { get; set; }

        /// <summary>
        /// Which part of the medal the layer belongs to: <c>1</c> the ribbon, <c>2</c> the medal.
        /// </summary>
        [JsonProperty("part")]
        public long Part { get; set; }
    }
}
