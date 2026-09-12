using ESI.NET.Models.Wallet;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class WalletLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public WalletLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /characters/{character_id}/wallet/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<decimal>> CharacterWallet(EsiCallOptions options)
            => await Execute<decimal>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/wallet/", replacements: new Dictionary<string, string>()
            {
                { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
            }, options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/wallet/journal/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<JournalEntry>>> CharacterJournal(EsiCallOptions options)
            => await Execute<List<JournalEntry>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/wallet/journal/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /characters/{character_id}/wallet/transactions/
        /// </summary>
        /// <param name="fromId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// /corporations/{corporation_id}/wallets/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Wallet>>> CorporationWallets(EsiCallOptions options)
            => await Execute<List<Wallet>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/wallets/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/wallets/{division}/journal/
        /// </summary>
        /// <param name="division"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<JournalEntry>>> CorporationJournal(long division, EsiCallOptions options)
            => await Execute<List<JournalEntry>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/wallets/{division}/journal/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "division", division.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/wallets/{division}/transactions/
        /// </summary>
        /// <param name="division"></param>
        /// <param name="fromId"></param>
        /// <returns></returns>
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