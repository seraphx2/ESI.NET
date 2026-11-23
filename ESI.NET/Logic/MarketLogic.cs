using ESI.NET.Enumerations;
using ESI.NET.Models.Market;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Market endpoints. Reach them through <see cref="EsiClient.Market"/>.
    /// </summary>
    public class MarketLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Market endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public MarketLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>List market prices. Return a list of prices.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /markets/prices</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Price"/>.</returns>
        public async Task<EsiResponse<List<Price>>> Prices(EsiCallOptions options = null)
            => await Execute<List<Price>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/prices/",
                options: options).ConfigureAwait(false);


        /// <summary>Return a list of orders in a region.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /markets/{region_id}/orders</c>.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="regionId">Return orders in this region.</param>
        /// <param name="orderType">Filter buy/sell orders, return all orders by default. If you
        /// query without type_id, we always return both buy and sell orders.</param>
        /// <param name="typeId">Return orders only for this type.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>, <see cref="EsiCallOptions.BypassCache"/>, and
        /// <see cref="EsiCallOptions.Page"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Order"/>.</returns>
        public async Task<EsiResponse<List<Order>>> RegionOrders(
            long regionId,
            MarketOrderType orderType = MarketOrderType.All,
            long? typeId = null,
            EsiCallOptions options = null)
        {
            var parameters = new List<string>() { $"order_type={orderType.ToEsiValue()}" };

            if (typeId != null)
                parameters.Add($"type_id={typeId}");

            var response = await Execute<List<Order>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/{region_id}/orders/",
                replacements: new Dictionary<string, string>()
                {
                    { "region_id", regionId.ToString(CultureInfo.InvariantCulture) }
                },
                parameters: parameters.ToArray(),
                options: options).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// List historical market statistics in a region. Return a list of historical market
        /// statistics for the specified type in a region This route expires daily at 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /markets/{region_id}/history</c>.</para>
        /// </remarks>
        /// <param name="regionId">Return statistics in this region.</param>
        /// <param name="typeId">Return statistics for this type.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Statistic"/>.</returns>
        public async Task<EsiResponse<List<Statistic>>> TypeHistoryInRegion(long regionId, long typeId, EsiCallOptions options = null)
            => await Execute<List<Statistic>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/{region_id}/history/",
                replacements: new Dictionary<string, string>()
                {
                    { "region_id", regionId.ToString(CultureInfo.InvariantCulture) }
                },
                parameters: new string[]
                {
                    $"type_id={typeId}"
                },
                options: options).ConfigureAwait(false);


        /// <summary>Return all orders in a structure.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /markets/structures/{structure_id}</c>.</para>
        /// <para>Requires the <c>esi-markets.structure_markets.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="structureId">Return orders in this structure.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call. <see cref="EsiCallOptions.Page"/> picks
        /// the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Order"/>.</returns>
        public async Task<EsiResponse<List<Order>>> StructureOrders(long structureId, EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/markets/structures/{structure_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "structure_id", structureId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get item groups. Get a list of item groups This route expires daily at 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /markets/groups</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <c>long</c> values.</returns>
        public async Task<EsiResponse<long[]>> Groups(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/groups/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Get item group information. Get information on an item group This route expires daily at
        /// 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /markets/groups/{market_group_id}</c>.</para>
        /// </remarks>
        /// <param name="marketGroupId">An Eve item group ID.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Group"/>.</returns>
        public async Task<EsiResponse<Group>> Group(long marketGroupId, EsiCallOptions options = null)
            => await Execute<Group>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/groups/{market_group_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "market_group_id", marketGroupId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// List open orders from a character. List open market orders placed by a character.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/orders</c>.</para>
        /// <para>Requires the <c>esi-markets.read_character_orders.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Order"/>.</returns>
        public async Task<EsiResponse<List<Order>>> CharacterOrders(EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/orders/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List historical orders by a character. List cancelled and expired market orders placed
        /// by a character up to 90 days in the past.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/orders/history</c>.</para>
        /// <para>Requires the <c>esi-markets.read_character_orders.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{character_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Order"/>.</returns>
        public async Task<EsiResponse<List<Order>>> CharacterOrderHistory(EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/orders/history/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List type IDs relevant to a market. Return a list of type IDs that have active orders in
        /// the region, for efficient market indexing.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /markets/{region_id}/types</c>.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="regionId">Return statistics in this region.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>, <see cref="EsiCallOptions.BypassCache"/>, and
        /// <see cref="EsiCallOptions.Page"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <c>long</c> values.</returns>
        public async Task<EsiResponse<long[]>> Types(long regionId, EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/{region_id}/types/",
                replacements: new Dictionary<string, string>()
                {
                    { "region_id", regionId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List open orders from a corporation. List open market orders placed on behalf of a
        /// corporation.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/orders</c>.</para>
        /// <para>Requires the <c>esi-markets.read_corporation_orders.v1</c> scope.</para>
        /// <para>The character must also hold one of these corporation roles: <c>Accountant</c>,
        /// <c>Trader</c>.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Order"/>.</returns>
        public async Task<EsiResponse<List<Order>>> CorporationOrders(EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/orders/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List historical orders from a corporation. List cancelled and expired market orders
        /// placed on behalf of a corporation up to 90 days in the past.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/orders/history</c>.</para>
        /// <para>Requires the <c>esi-markets.read_corporation_orders.v1</c> scope.</para>
        /// <para>The character must also hold one of these corporation roles: <c>Accountant</c>,
        /// <c>Trader</c>.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Order"/>.</returns>
        public async Task<EsiResponse<List<Order>>> CorporationOrderHistory(EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/orders/history/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}