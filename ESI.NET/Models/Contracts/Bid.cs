using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Contracts
{
    public class Bid
    {
        [JsonProperty("bid_id")]
        public long BidId { get; set; }

        [JsonProperty("bidder_id")]
        public long BidderId { get; set; }

        [JsonProperty("date_bid")]
        public DateTime DateBid { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}
