using ESI.NET.Enumerations;
using ESI.NET.Models.Market;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Interfaces.Logic;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class MarketLogic : IMarketLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly AuthorizedCharacterData _data;
        private readonly int character_id, corporation_id;

        public MarketLogic(HttpClient client, EsiConfig config, AuthorizedCharacterData data = null, string eTag = null,
            CancellationToken cancellationToken = default)
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
        public async Task<EsiResponse<List<Price>>> Prices(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Price>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/prices/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /markets/{region_id}/orders/
        /// </summary>
        /// <param name="region_id"></param>
        /// <param name="order_type"></param>
        /// <param name="page"></param>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Order>>> RegionOrders(
            int region_id,
            MarketOrderType order_type = MarketOrderType.All,
            int page = 1,
            int? type_id = null,
            string eTag = null,
            CancellationToken cancellationToken = default)
        {
            var parameters = new List<string>() {$"order_type={order_type.ToEsiValue()}"};
            parameters.Add($"page={page}");

            if (type_id != null)
                parameters.Add($"type_id={type_id}");

            var response = await Execute<List<Order>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/markets/{region_id}/orders/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"region_id", region_id.ToString()}
                },
                parameters: parameters.ToArray());

            return response;
        }

        /// <summary>
        /// /markets/{region_id}/history/
        /// </summary>
        /// <param name="region_id"></param>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Statistic>>> TypeHistoryInRegion(int region_id, int type_id,
            string eTag = null, CancellationToken cancellationToken = default)
            => await Execute<List<Statistic>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/markets/{region_id}/history/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"region_id", region_id.ToString()}
                },
                parameters: new string[]
                {
                    $"type_id={type_id}"
                });

        /// <summary>
        /// /markets/structures/{structure_id}/
        /// </summary>
        /// <param name="structure_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Order>>> StructureOrders(long structure_id, int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/markets/structures/{structure_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"structure_id", structure_id.ToString()}
                },
                parameters: new string[]
                {
                    $"page={page}"
                },
                token: _data.Token);

        /// <summary>
        /// /markets/groups/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Groups(string eTag = null, CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/groups/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /markets/groups/{market_group_id}/
        /// </summary>
        /// <param name="market_group_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Group>> Group(int market_group_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Group>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/markets/groups/{market_group_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"market_group_id", market_group_id.ToString()}
                });

        /// <summary>
        /// /characters/{character_id}/orders/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Order>>> CharacterOrders(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/orders/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"character_id", character_id.ToString()}
                },
                token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/orders/history/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Order>>> CharacterOrderHistory(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/orders/history/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"character_id", character_id.ToString()}
                },
                parameters: new string[]
                {
                    $"page={page}"
                },
                token: _data.Token);

        /// <summary>
        /// /markets/{region_id}/types/
        /// </summary>
        /// <param name="region_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Types(int region_id, int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/markets/{region_id}/types/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"region_id", region_id.ToString()}
                },
                parameters: new string[]
                {
                    $"page={page}"
                });

        /// <summary>
        /// /corporations/{corporation_id}/orders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Order>>> CorporationOrders(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/orders/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"corporation_id", corporation_id.ToString()}
                },
                parameters: new string[]
                {
                    $"page={page}"
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/orders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Order>>> CorporationOrderHistory(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/orders/history/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"corporation_id", corporation_id.ToString()}
                },
                parameters: new string[]
                {
                    $"page={page}"
                },
                token: _data.Token);
    }
}