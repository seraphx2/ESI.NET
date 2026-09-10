using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Character
{
    public class Medal
    {
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("graphics")]
        public List<GraphicLayer> Graphics { get; set; } = new List<GraphicLayer>();

        [JsonProperty("issuer_id")]
        public long IssuerId { get; set; }

        [JsonProperty("medal_id")]
        public long MedalId { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class GraphicLayer
    {
        [JsonProperty("color")]
        public long Color { get; set; }

        [JsonProperty("graphic")]
        public string Graphic { get; set; }

        [JsonProperty("layer")]
        public long Layer { get; set; }

        [JsonProperty("part")]
        public long Part { get; set; }
    }
}
