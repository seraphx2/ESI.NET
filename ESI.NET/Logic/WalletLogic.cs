using ESI.NET.Models.Wallet;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Wallet endpoints. Reach them through <see cref="EsiClient.Wallet"/>.
    /// </summary>
    public class WalletLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Wallet endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public WalletLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>Returns a character's wallet balance.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/wallet</c>.</para>
        /// <para>Requires the <c>esi-wallet.read_character_wallet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <c>decimal</c> value.</returns>
        public async Task<EsiResponse<decimal>> CharacterWallet(EsiCallOptions options)
            => await Execute<decimal>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/wallet/", replacements: new Dictionary<string, string>()
            {
                { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
            }, options: options).ConfigureAwait(false);

        /// <summary>
        /// Get character wallet journal. Retrieve the given character's wallet journal going 30
        /// days back.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/wallet/journal</c>.</para>
        /// <para>Requires the <c>esi-wallet.read_character_wallet.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{character_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="JournalEntry"/>.</returns>
        public async Task<EsiResponse<List<JournalEntry>>> CharacterJournal(EsiCallOptions options)
            => await Execute<List<JournalEntry>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/wallet/journal/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>Get wallet transactions of a character.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/wallet/transactions</c>.</para>
        /// <para>Requires the <c>esi-wallet.read_character_wallet.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fromId">Only show transactions happened before the one referenced by this
        /// id.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Transaction"/>.</returns>
        public async Task<EsiResponse<List<Transaction>>> CharacterTransactions(long fromId, EsiCallOptions options)
            => await Execute<List<Transaction>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/wallet/transactions/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                parameters: new string[]
                {
                    $"from_id={fromId}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>Returns a corporation's wallet balance. Get a corporation's wallets.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/wallets</c>.</para>
        /// <para>Requires the <c>esi-wallet.read_corporation_wallets.v1</c> scope.</para>
        /// <para>The character must also hold one of these corporation roles: <c>Accountant</c>,
        /// <c>Junior_Accountant</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Wallet"/>.</returns>
        public async Task<EsiResponse<List<Wallet>>> CorporationWallets(EsiCallOptions options)
            => await Execute<List<Wallet>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/wallets/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get corporation wallet journal. Retrieve the given corporation's wallet journal for the
        /// given division going 30 days back.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/wallets/{division}/journal</c>.</para>
        /// <para>Requires the <c>esi-wallet.read_corporation_wallets.v1</c> scope.</para>
        /// <para>The character must also hold one of these corporation roles: <c>Accountant</c>,
        /// <c>Junior_Accountant</c>.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="division">Wallet key of the division to fetch journals from.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="JournalEntry"/>.</returns>
        public async Task<EsiResponse<List<JournalEntry>>> CorporationJournal(long division, EsiCallOptions options)
            => await Execute<List<JournalEntry>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/wallets/{division}/journal/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "division", division.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Get wallet transactions of a corporation.</summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /corporations/{corporation_id}/wallets/{division}/transactions</c>.</para>
        /// <para>Requires the <c>esi-wallet.read_corporation_wallets.v1</c> scope.</para>
        /// <para>The character must also hold one of these corporation roles: <c>Accountant</c>,
        /// <c>Junior_Accountant</c>.</para>
        /// </remarks>
        /// <param name="division">Wallet key of the division to fetch journals from.</param>
        /// <param name="fromId">Only show journal entries happened before the transaction
        /// referenced by this id.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Transaction"/>.</returns>
        public async Task<EsiResponse<List<Transaction>>> CorporationTransactions(long division, long fromId, EsiCallOptions options)
            => await Execute<List<Transaction>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/wallets/{division}/transactions/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "division", division.ToString(CultureInfo.InvariantCulture) }
                },
                parameters: new string[]
                {
                    $"from_id={fromId}"
                },
                options: options).ConfigureAwait(false);
    }
}