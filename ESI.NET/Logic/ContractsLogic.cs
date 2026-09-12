using ESI.NET.Models.Contracts;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class ContractsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public ContractsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /contracts/public/{region_id}/
        /// </summary>
        /// <param name="region_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Contract>>> Contracts(long region_id, EsiCallOptions options = null)
            => await Execute<List<Contract>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/contracts/public/{region_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "region_id", region_id.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /contracts/public/items/{contract_id}/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<ContractItem>>> ContractItems(long contract_id, EsiCallOptions options = null)
            => await Execute<List<ContractItem>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/contracts/public/items/{contract_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "contract_id", contract_id.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// "/contracts/public/bids/{contract_id}/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Bid>>> ContractBids(long contract_id, EsiCallOptions options = null)
            => await Execute<List<Bid>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/contracts/public/bids/{contract_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "contract_id", contract_id.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/contracts/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Contract>>> CharacterContracts(EsiCallOptions options)
            => await Execute<List<Contract>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/contracts/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/contracts/{contract_id}/items/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<ContractItem>>> CharacterContractItems(long contract_id, EsiCallOptions options)
            => await Execute<List<ContractItem>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/contracts/{contract_id}/items/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "contract_id", contract_id.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/contracts/{contract_id}/bids/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Bid>>> CharacterContractBids(long contract_id, EsiCallOptions options)
            => await Execute<List<Bid>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/contracts/{contract_id}/bids/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "contract_id", contract_id.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/contracts/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Contract>>> CorporationContracts(EsiCallOptions options)
            => await Execute<List<Contract>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/contracts/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/contracts/{contract_id}/items/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<ContractItem>>> CorporationContractItems(long contract_id, EsiCallOptions options)
            => await Execute<List<ContractItem>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/contracts/{contract_id}/items/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "contract_id", contract_id.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/contracts/{contract_id}/bids/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Bid>>> CorporationContractBids(long contract_id, EsiCallOptions options)
            => await Execute<List<Bid>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/contracts/{contract_id}/bids/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "contract_id", contract_id.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}