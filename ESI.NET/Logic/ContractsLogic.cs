using ESI.NET.Models.Contracts;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Contracts endpoints. Reach them through <see cref="EsiClient.Contracts"/>.
    /// </summary>
    public class ContractsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Contracts endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public ContractsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// Get public contracts. Returns a paginated list of all public contracts in the given
        /// region.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /contracts/public/{region_id}</c>.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="regionId">An EVE region id.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>, <see cref="EsiCallOptions.BypassCache"/>, and
        /// <see cref="EsiCallOptions.Page"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Contract"/>.</returns>
        public async Task<EsiResponse<List<Contract>>> Contracts(long regionId, EsiCallOptions options = null)
            => await Execute<List<Contract>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/contracts/public/{region_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "region_id", regionId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Lists items of a public contract.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /contracts/public/items/{contract_id}</c>.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="contractId">ID of a contract.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>, <see cref="EsiCallOptions.BypassCache"/>, and
        /// <see cref="EsiCallOptions.Page"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="ContractItem"/>.</returns>
        public async Task<EsiResponse<List<ContractItem>>> ContractItems(long contractId, EsiCallOptions options = null)
            => await Execute<List<ContractItem>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/contracts/public/items/{contract_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "contract_id", contractId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Lists bids on a public auction contract.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /contracts/public/bids/{contract_id}</c>.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="contractId">ID of a contract.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>, <see cref="EsiCallOptions.BypassCache"/>, and
        /// <see cref="EsiCallOptions.Page"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Bid"/>.</returns>
        public async Task<EsiResponse<List<Bid>>> ContractBids(long contractId, EsiCallOptions options = null)
            => await Execute<List<Bid>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/contracts/public/bids/{contract_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "contract_id", contractId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get contracts. Returns contracts available to a character, only if the character is
        /// issuer, acceptor or assignee. Only returns contracts no older than 30 days, or if the
        /// status is "in_progress".
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/contracts</c>.</para>
        /// <para>Requires the <c>esi-contracts.read_character_contracts.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{character_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Contract"/>.</returns>
        public async Task<EsiResponse<List<Contract>>> CharacterContracts(EsiCallOptions options)
            => await Execute<List<Contract>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/contracts/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Lists items of a particular contract.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/contracts/{contract_id}/items</c>.</para>
        /// <para>Requires the <c>esi-contracts.read_character_contracts.v1</c> scope.</para>
        /// </remarks>
        /// <param name="contractId">ID of a contract.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="ContractItem"/>.</returns>
        public async Task<EsiResponse<List<ContractItem>>> CharacterContractItems(long contractId, EsiCallOptions options)
            => await Execute<List<ContractItem>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/contracts/{contract_id}/items/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "contract_id", contractId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Get contract bids. Lists bids on a particular auction contract.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/contracts/{contract_id}/bids</c>.</para>
        /// <para>Requires the <c>esi-contracts.read_character_contracts.v1</c> scope.</para>
        /// </remarks>
        /// <param name="contractId">ID of a contract.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Bid"/>.</returns>
        public async Task<EsiResponse<List<Bid>>> CharacterContractBids(long contractId, EsiCallOptions options)
            => await Execute<List<Bid>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/contracts/{contract_id}/bids/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "contract_id", contractId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get corporation contracts. Returns contracts available to a corporation, only if the
        /// corporation is issuer, acceptor or assignee. Only returns contracts no older than 30
        /// days, or if the status is "in_progress".
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/contracts</c>.</para>
        /// <para>Requires the <c>esi-contracts.read_corporation_contracts.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Contract"/>.</returns>
        public async Task<EsiResponse<List<Contract>>> CorporationContracts(EsiCallOptions options)
            => await Execute<List<Contract>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/contracts/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Get corporation contract items. Lists items of a particular contract.</summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /corporations/{corporation_id}/contracts/{contract_id}/items</c>.</para>
        /// <para>Requires the <c>esi-contracts.read_corporation_contracts.v1</c> scope.</para>
        /// </remarks>
        /// <param name="contractId">ID of a contract.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="ContractItem"/>.</returns>
        public async Task<EsiResponse<List<ContractItem>>> CorporationContractItems(long contractId, EsiCallOptions options)
            => await Execute<List<ContractItem>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/contracts/{contract_id}/items/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "contract_id", contractId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get corporation contract bids. Lists bids on a particular auction contract.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /corporations/{corporation_id}/contracts/{contract_id}/bids</c>.</para>
        /// <para>Requires the <c>esi-contracts.read_corporation_contracts.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="contractId">ID of a contract.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Bid"/>.</returns>
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