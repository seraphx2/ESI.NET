using Newtonsoft.Json;

namespace ESI.NET.Models.Market
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.MarketLogic.Group"/>.</summary>
    public class Group
    {
        /// <summary>ID of the market group.</summary>
        [JsonProperty("market_group_id")]
        public long MarketGroupId { get; set; }

        /// <summary>The market group's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The market group's description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Type IDs of the items in this group.</summary>
        [JsonProperty("types")]
        public long[] Types { get; set; }

        /// <summary>ID of the parent market group. Absent for a top-level group.</summary>
        [JsonProperty("parent_group_id")]
        public long ParentGroupId { get; set; }
    }
}
