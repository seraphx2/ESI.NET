using ESI.NET.Enumerations;
using ESI.NET.Models.Market;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class MarketLogic
    {
        private HttpClient _client;
        private ESIConfig _config;
        private AuthorizedCharacterData _data;
        private int corporation_id, character_id;

        public MarketLogic(HttpClient client, ESIConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
            {
                corporation_id = data.CorporationID;
                character_id = data.CharacterID;
            }
        }

        /// <summary>
        /// /markets/prices/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Price>>> Prices()
            => await Execute<List<Price>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/markets/prices/");

        /// <summary>
        /// /markets/{region_id}/orders/
        /// </summary>
        /// <param name="region_id"></param>
        /// <param name="order_type"></param>
        /// <param name="page"></param>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Order>>> RegionOrders(
            int region_id, 
            MarketOrderType order_type = MarketOrderType.All, 
            int page = 1, 
            int? type_id = null)
        {
            var parameters = new List<string>() { $"order_type={order_type.ToEsiValue()}" };
            parameters.Add($"page={page}");

            if (type_id != null)
                parameters.Add($"type_id={type_id}");

            var response = await Execute<List<Order>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, $"/markets/{region_id}/orders/", parameters.ToArray());

            return response;
        }

        /// <summary>
        /// /markets/{region_id}/history/
        /// </summary>
        /// <param name="region_id"></param>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Statistic>>> TypeHistoryInRegion(int region_id, int type_id)
            => await Execute<List<Statistic>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, $"/markets/{region_id}/history/", new string[]
            {
                $"type_id={type_id}"
            });

        /// <summary>
        /// /markets/structures/{structure_id}/
        /// </summary>
        /// <param name="structure_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Order>>> StructureOrders(long structure_id, int page = 1)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/markets/structures/{structure_id}/", new string[]
            {
                $"page={page}"
            }, token: _data.Token);

        /// <summary>
        /// /markets/groups/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Groups()
            => await Execute<List<int>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/markets/groups/");

        /// <summary>
        /// /markets/groups/{market_group_id}/
        /// </summary>
        /// <param name="market_group_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Group>> Group(int market_group_id)
            => await Execute<Group>(_client, _config, RequestSecurity.Public, RequestMethod.GET, $"/markets/groups/{market_group_id}/");

        /// <summary>
        /// /characters/{character_id}/orders/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Order>>> CharacterOrders()
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/orders/", token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/orders/history/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Order>>> CharacterOrderHistory(int page = 1)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/orders/history/", new string[]
            {
                $"page={page}"
            }, token: _data.Token);

        /// <summary>
        /// /markets/{region_id}/types/
        /// </summary>
        /// <param name="region_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Types(int region_id, int page = 1)
            => await Execute<List<int>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, $"/markets/{region_id}/types/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /corporations/{corporation_id}/orders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Order>>> CorporationOrders(int page = 1)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/orders/", new string[]
            {
                $"page={page}"
            }, token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/orders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Order>>> CorporationOrderHistory(int page = 1)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/orders/history/", new string[]
            {
                $"page={page}"
            }, token: _data.Token);
    }
}