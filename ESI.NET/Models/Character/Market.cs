// Obsolete and undocumented on purpose: removed with the next compatibility-date release.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using Newtonsoft.Json;

namespace ESI.NET.Models.Character
{
    /// <summary>
    /// Part of ESI's removed character statistics endpoint (<c>/characters/{character_id}/stats/</c>).
    /// No ESI.NET method returns it.
    /// </summary>
    [Obsolete("No ESI endpoint returns this: CCP removed the character statistics endpoint. Will be removed with the next compatibility-date release.")]
    public class Market
    {
        [JsonProperty("accept_contracts_courier")]
        public long AcceptContractsCourier { get; set; }

        [JsonProperty("accept_contracts_item_exchange")]
        public long AcceptContractsItemExchange { get; set; }

        [JsonProperty("buy_orders_placed")]
        public long BuyOrdersPlaced { get; set; }

        [JsonProperty("cancel_market_order")]
        public long CancelMarketOrder { get; set; }

        [JsonProperty("create_contracts_auction")]
        public long CreateContractsAuction { get; set; }

        [JsonProperty("create_contracts_courier")]
        public long CreateContractsCourier { get; set; }

        [JsonProperty("create_contracts_item_exchange")]
        public long CreateContractsItemExchange { get; set; }

        [JsonProperty("deliver_courier_contract")]
        public long DeliverCourierContract { get; set; }

        [JsonProperty("isk_gained")]
        public long IskGained { get; set; }

        [JsonProperty("isk_spent")]
        public long IskSpent { get; set; }

        [JsonProperty("modify_market_order")]
        public long ModifyMarketOrder { get; set; }

        [JsonProperty("search_contracts")]
        public long SearchContracts { get; set; }

        [JsonProperty("sell_orders_placed")]
        public long SellOrdersPlaced { get; set; }
    }
}
