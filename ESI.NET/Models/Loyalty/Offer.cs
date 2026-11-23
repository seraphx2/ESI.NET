using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Loyalty
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.LoyaltyLogic.Offers"/>.</summary>
    public class Offer
    {
        /// <summary>ID of the offer.</summary>
        [JsonProperty("offer_id")]
        public long OfferId { get; set; }

        /// <summary>Type ID of the item the offer gives.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>How many of the item the offer gives.</summary>
        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        /// <summary>Loyalty point cost of the offer.</summary>
        [JsonProperty("lp_cost")]
        public long LpCost { get; set; }

        /// <summary>ISK cost of the offer.</summary>
        [JsonProperty("isk_cost")]
        public long IskCost { get; set; }

        /// <summary>Analysis kredit cost.</summary>
        [JsonProperty("ak_cost")]
        public long AkCost { get; set; }

        /// <summary>Items that must be handed in along with the ISK and loyalty points.</summary>
        [JsonProperty("required_items")]
        public List<Item> RequiredItems { get; set; } = new List<Item>();
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Loyalty.Offer.RequiredItems"/>.</summary>
    public class Item
    {
        /// <summary>Type ID of the item that must be handed in.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>How many of the item must be handed in.</summary>
        [JsonProperty("quantity")]
        public long Quantity { get; set; }
    }
}
