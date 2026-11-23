using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Market
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.MarketLogic.CharacterOrderHistory"/>,
    /// <see cref="ESI.NET.Logic.MarketLogic.CharacterOrders"/>,
    /// <see cref="ESI.NET.Logic.MarketLogic.CorporationOrderHistory"/>,
    /// <see cref="ESI.NET.Logic.MarketLogic.CorporationOrders"/>,
    /// <see cref="ESI.NET.Logic.MarketLogic.RegionOrders"/>, and
    /// <see cref="ESI.NET.Logic.MarketLogic.StructureOrders"/>.
    /// </summary>
    public class Order
    {
        /// <summary>Unique order ID.</summary>
        [JsonProperty("order_id")]
        public long OrderId { get; set; }

        /// <summary>The type ID of the item transacted in this order.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>
        /// ID of the region the order was placed in. Only returned by the character orders
        /// endpoints.
        /// </summary>
        [JsonProperty("region_id")]
        public long RegionId { get; set; }

        /// <summary>The solar system this order was placed.</summary>
        [JsonProperty("system_id")]
        public long SystemId { get; set; }

        /// <summary>ID of the location where order was placed.</summary>
        [JsonProperty("location_id")]
        public long LocationId { get; set; }

        /// <summary>
        /// Valid order range, numbers are ranges in jumps. One of <c>1</c>, <c>10</c>, <c>2</c>,
        /// <c>20</c>, <c>3</c>, <c>30</c>, <c>4</c>, <c>40</c>, <c>5</c>, <c>region</c>,
        /// <c>solarsystem</c>, <c>station</c>.
        /// </summary>
        [JsonProperty("range")]
        public string Range { get; set; }

        /// <summary>True if the order is a bid (buy) order.</summary>
        [JsonProperty("is_buy_order")]
        public bool IsBuyOrder { get; set; }

        /// <summary>Cost per unit for this order.</summary>
        [JsonProperty("price")]
        public decimal Price { get; set; }

        /// <summary>Quantity of items required or offered at time order was placed.</summary>
        [JsonProperty("volume_total")]
        public long VolumeTotal { get; set; }

        /// <summary>Quantity of items still required or offered.</summary>
        [JsonProperty("volume_remain")]
        public long VolumeRemain { get; set; }

        /// <summary>Date and time when this order was issued.</summary>
        [JsonProperty("issued")]
        public DateTime Issued { get; set; }

        /// <summary>
        /// ID of the character who placed the order. Only returned by the corporation orders
        /// endpoints.
        /// </summary>
        [JsonProperty("issued_by")]
        public long IssuedBy { get; set; }

        /// <summary>Current order state. One of <c>cancelled</c>, <c>expired</c>.</summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// For buy orders, the minimum quantity that will be accepted in a matching sell order.
        /// </summary>
        [JsonProperty("min_volume")]
        public long MinVolume { get; set; }

        /// <summary>
        /// Number of days for which order is valid (starting from the issued date). An order
        /// expires at time issued + duration.
        /// </summary>
        [JsonProperty("duration")]
        public long Duration { get; set; }

        /// <summary>
        /// Signifies whether the buy/sell order was placed on behalf of a corporation.
        /// </summary>
        [JsonProperty("is_corporation")]
        public bool IsCorporation { get; set; }

        /// <summary>For buy orders, the amount of ISK in escrow.</summary>
        [JsonProperty("escrow")]
        public decimal Escrow { get; set; }

        /// <summary>
        /// The corporation wallet division the order uses. Only returned by the corporation orders
        /// endpoints.
        /// </summary>
        [JsonProperty("wallet_division")]
        public long WalletDivision { get; set; }
    }
}