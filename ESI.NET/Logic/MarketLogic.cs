using ESI.NET.Enumerations;
using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Market;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class MarketLogic : IMarketLogic
    {
        private ESIConfig _config;
        private int corporation_id, character_id;

        public MarketLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                corporation_id = _config.AuthorizedCharacter.CorporationID;
                character_id = _config.AuthorizedCharacter.CharacterID;
            }
        }

        /// <summary>
        /// /markets/prices/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Price>>> Prices()
        {
            var endpoint = "/markets/prices/";
            var response = await Execute<List<Price>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

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
            MarketOrderType order_type = MarketOrderType.all, 
            int page = 1, 
            int? type_id = null)
        {
            var parameters = new List<string>() { $"order_type={order_type}" };
            parameters.Add($"page={page}");

            if (type_id != null)
                parameters.Add($"type_id={type_id}");

            var endpoint = $"/markets/{region_id}/orders/";
            var response = await Execute<List<Order>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint, parameters.ToArray());

            return response;
        }

        /// <summary>
        /// /markets/{region_id}/history/
        /// </summary>
        /// <param name="region_id"></param>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Statistic>>> TypeHistoryInRegion(int region_id, int type_id)
        {
            var endpoint = $"/markets/{region_id}/history/";
            var response = await Execute<List<Statistic>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint, new string[]
            {
                $"type_id={type_id}"
            });

            return response;
        }

        /// <summary>
        /// /markets/structures/{structure_id}/
        /// </summary>
        /// <param name="structure_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Order>>> StructureOrders(long structure_id, int page = 1)
        {
            var endpoint = $"/markets/structures/{structure_id}/";
            var response = await Execute<List<Order>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }

        /// <summary>
        /// /markets/groups/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Groups()
        {
            var endpoint = "/markets/groups/";
            var response = await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /markets/groups/{market_group_id}/
        /// </summary>
        /// <param name="market_group_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Group>> Group(int market_group_id)
        {
            var endpoint = $"/markets/groups/{market_group_id}/";
            var response = await Execute<Group>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/orders/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Order>>> CharacterOrders()
        {
            var endpoint = $"/characters/{character_id}/orders/";
            var response = await Execute<List<Order>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/orders/history/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Order>>> CharacterOrderHistory(int page = 1)
        {
            var endpoint = $"/characters/{character_id}/orders/history/";
            var response = await Execute<List<Order>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }

        /// <summary>
        /// /markets/{region_id}/types/
        /// </summary>
        /// <param name="region_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Types(int region_id, int page = 1)
        {
            var endpoint = $"/markets/{region_id}/types/";
            var response = await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }

        /// <summary>
        /// /corporations/{corporation_id}/orders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Order>>> CorporationOrders(int page = 1)
        {
            var endpoint = $"/corporations/{corporation_id}/orders/";
            var response = await Execute<List<Order>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }

        /// <summary>
        /// /corporations/{corporation_id}/orders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Order>>> CorporationOrderHistory(int page = 1)
        {
            var endpoint = $"/corporations/{corporation_id}/orders/history/";
            var response = await Execute<List<Order>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }
    }
}
