using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Loyalty
{
    public class Offer
    {
        [JsonProperty("offer_id")]
        public long OfferId { get; set; }

        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("lp_cost")]
        public long LpCost { get; set; }

        [JsonProperty("isk_cost")]
        public long IskCost { get; set; }

        [JsonProperty("ak_cost")]
        public long AkCost { get; set; }

        [JsonProperty("required_items")]
        public List<Item> RequiredItems { get; set; } = new List<Item>();
    }

    public class Item
    {
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }
    }
}
