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
        /// <param name="regionId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Contract>>> Contracts(long regionId, EsiCallOptions options = null)
            => await Execute<List<Contract>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/contracts/public/{region_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "region_id", regionId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /contracts/public/items/{contract_id}/
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<ContractItem>>> ContractItems(long contractId, EsiCallOptions options = null)
            => await Execute<List<ContractItem>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/contracts/public/items/{contract_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "contract_id", contractId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// "/contracts/public/bids/{contract_id}/
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Bid>>> ContractBids(long contractId, EsiCallOptions options = null)
            => await Execute<List<Bid>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/contracts/public/bids/{contract_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "contract_id", contractId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/contracts/
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
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
        /// <param name="contractId"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<List<ContractItem>>> CharacterContractItems(long contractId, EsiCallOptions options)
            => await Execute<List<ContractItem>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/contracts/{contract_id}/items/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "contract_id", contractId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/contracts/{contract_id}/bids/
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<List<Bid>>> CharacterContractBids(long contractId, EsiCallOptions options)
            => await Execute<List<Bid>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/contracts/{contract_id}/bids/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "contract_id", contractId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/contracts/
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
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
        /// <param name="contractId"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<List<ContractItem>>> CorporationContractItems(long contractId, EsiCallOptions options)
            => await Execute<List<ContractItem>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/contracts/{contract_id}/items/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "contract_id", contractId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/contracts/{contract_id}/bids/
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<List<Bid>>> CorporationContractBids(long contractId, EsiCallOptions options)
            => await Execute<List<Bid>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/contracts/{contract_id}/bids/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "contract_id", contractId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}