using Newtonsoft.Json;

namespace ESI.NET.Models.Market
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.MarketLogic.Prices"/>.</summary>
    public class Price
    {
        /// <summary>Type ID of the item.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>The item's average market price across New Eden over recent days.</summary>
        [JsonProperty("average_price")]
        public decimal AveragePrice { get; set; }

        /// <summary>
        /// CCP's adjusted price for the item. It is not a market price: the game uses it to work
        /// out industry job costs and some taxes.
        /// </summary>
        [JsonProperty("adjusted_price")]
        public decimal AdjustedPrice { get; set; }
    }
}
