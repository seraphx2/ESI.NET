using ESI.NET.Enumerations;
using ESI.NET.Models.Market;
using System.Collections.Generic;
using System.Globalization;
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
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /markets/{region_id}/orders/
        /// </summary>
        /// <param name="regionId"></param>
        /// <param name="orderType"></param>
        /// <param name="page"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
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
        /// /markets/{region_id}/history/
        /// </summary>
        /// <param name="regionId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
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


        /// <summary>
        /// /markets/structures/{structure_id}/
        /// </summary>
        /// <param name="structureId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Order>>> StructureOrders(long structureId, EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/markets/structures/{structure_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "structure_id", structureId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /markets/groups/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Groups(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/groups/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /markets/groups/{market_group_id}/
        /// </summary>
        /// <param name="marketGroupId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Group>> Group(long marketGroupId, EsiCallOptions options = null)
            => await Execute<Group>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/groups/{market_group_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "market_group_id", marketGroupId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /characters/{character_id}/orders/
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<List<Order>>> CharacterOrders(EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/orders/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/orders/history/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<List<Order>>> CharacterOrderHistory(EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/orders/history/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /markets/{region_id}/types/
        /// </summary>
        /// <param name="regionId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Types(long regionId, EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/markets/{region_id}/types/",
                replacements: new Dictionary<string, string>()
                {
                    { "region_id", regionId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/orders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<List<Order>>> CorporationOrders(EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/orders/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/orders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<List<Order>>> CorporationOrderHistory(EsiCallOptions options)
            => await Execute<List<Order>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/orders/history/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}