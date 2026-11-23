using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Market
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.MarketLogic.TypeHistoryInRegion"/>.
    /// </summary>
    public class Statistic
    {
        /// <summary>The date of this historical statistic entry.</summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>Total number of orders happened that day.</summary>
        [JsonProperty("order_count")]
        public long OrderCount { get; set; }

        /// <summary>Total.</summary>
        [JsonProperty("volume")]
        public long Volume { get; set; }

        /// <summary>Highest price that day.</summary>
        [JsonProperty("highest")]
        public decimal Highest { get; set; }

        /// <summary>Average price that day.</summary>
        [JsonProperty("average")]
        public decimal Average { get; set; }

        /// <summary>Lowest price that day.</summary>
        [JsonProperty("lowest")]
        public decimal Lowest { get; set; }
    }
}
