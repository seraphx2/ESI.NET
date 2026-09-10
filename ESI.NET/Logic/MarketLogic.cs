using ESI.NET.Enumerations;
using ESI.NET.Models.Market;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class MarketLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public MarketLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /markets/prices/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Price>>> Prices(EsiCallOptions options = null)
            => await Execute<List<Price>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/prices/",
                options: options);


        /// <summary>
        /// /markets/{region_id}/orders/
        /// </summary>
        /// <param name="region_id"></param>
        /// <param name="order_type"></param>
        /// <param name="page"></param>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Order>>> RegionOrders(
            long region_id,
            MarketOrderType order_type = MarketOrderType.All,
            long? type_id = null,
            EsiCallOptions options = null)
        {
            var parameters = new List<string>() { $"order_type={order_type.ToEsiValue()}" };

            if (type_id != null)
                parameters.Add($"type_id={type_id}");

            var response = await Execute<List<Order>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/{region_id}/orders/",
                replacements: new Dictionary<string, string>()
                {
                    { "region_id", region_id.ToString() }
                },
                parameters: parameters.ToArray(),
                options: options);

            return response;
        }

        /// <summary>
        /// /markets/{region_id}/history/
        /// </summary>
        /// <param name="region_id"></param>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Statistic>>> TypeHistoryInRegion(long region_id, long type_id, EsiCallOptions options = null)
            => await Execute<List<Statistic>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/{region_id}/history/",
                replacements: new Dictionary<string, string>()
                {
                    { "region_id", region_id.ToString() }
                },
                parameters: new string[]
                {
                    $"type_id={type_id}"
                },
                options: options);


        /// <summary>
        /// /markets/structures/{structure_id}/
        /// </summary>
        /// <param name="structure_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Order>>> StructureOrders(long structure_id, EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/markets/structures/{structure_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "structure_id", structure_id.ToString() }
                },
                options: options);

        /// <summary>
        /// /markets/groups/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Groups(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/groups/",
                options: options);


        /// <summary>
        /// /markets/groups/{market_group_id}/
        /// </summary>
        /// <param name="market_group_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Group>> Group(long market_group_id, EsiCallOptions options = null)
            => await Execute<Group>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/groups/{market_group_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "market_group_id", market_group_id.ToString() }
                },
                options: options);


        /// <summary>
        /// /characters/{character_id}/orders/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Order>>> CharacterOrders(EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/orders/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /characters/{character_id}/orders/history/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Order>>> CharacterOrderHistory(EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/orders/history/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);

        /// <summary>
        /// /markets/{region_id}/types/
        /// </summary>
        /// <param name="region_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Types(long region_id, EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/{region_id}/types/",
                replacements: new Dictionary<string, string>()
                {
                    { "region_id", region_id.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/orders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Order>>> CorporationOrders(EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/orders/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/orders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Order>>> CorporationOrderHistory(EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/orders/history/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);
    }
}