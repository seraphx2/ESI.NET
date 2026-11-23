using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Contracts
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.ContractsLogic.CharacterContractBids"/>,
    /// <see cref="ESI.NET.Logic.ContractsLogic.ContractBids"/>, and
    /// <see cref="ESI.NET.Logic.ContractsLogic.CorporationContractBids"/>.
    /// </summary>
    public class Bid
    {
        /// <summary>Unique ID for the bid.</summary>
        [JsonProperty("bid_id")]
        public long BidId { get; set; }

        /// <summary>Character ID of the bidder.</summary>
        [JsonProperty("bidder_id")]
        public long BidderId { get; set; }

        /// <summary>Datetime when the bid was placed.</summary>
        [JsonProperty("date_bid")]
        public DateTime DateBid { get; set; }

        /// <summary>The amount bid, in ISK.</summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}
