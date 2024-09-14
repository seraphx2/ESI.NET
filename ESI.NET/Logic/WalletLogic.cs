using ESI.NET.Models.SSO;
using ESI.NET.Models.Wallet;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Interfaces.Logic;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class WalletLogic : IWalletLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly AuthorizedCharacterData _data;
        private readonly int character_id, corporation_id;

        public WalletLogic(HttpClient client, EsiConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
            {
                character_id = data.CharacterID;
                corporation_id = data.CorporationID;
            }
        }

        /// <summary>
        /// /characters/{character_id}/wallet/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<decimal>> CharacterWallet(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<decimal>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/wallet/",
                eTag: eTag,
                cancellationToken: cancellationToken, replacements: new Dictionary<string, string>()
                {
                    {"character_id", character_id.ToString()}
                }, token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/wallet/journal/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<JournalEntry>>> CharacterJournal(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<JournalEntry>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/wallet/journal/",
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
        /// /characters/{character_id}/wallet/transactions/
        /// </summary>
        /// <param name="from_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Transaction>>> CharacterTransactions(long from_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Transaction>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/wallet/transactions/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"character_id", character_id.ToString()}
                },
                parameters: new string[]
                {
                    $"from_id={from_id}"
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/wallets/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Wallet>>> CorporationWallets(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Wallet>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/wallets/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"corporation_id", corporation_id.ToString()}
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/wallets/{division}/journal/
        /// </summary>
        /// <param name="division"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<JournalEntry>>> CorporationJournal(int division, int page = 1,
            string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<JournalEntry>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/wallets/{division}/journal/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"corporation_id", corporation_id.ToString()},
                    {"division", division.ToString()}
                },
                parameters: new string[]
                {
                    $"page={page}"
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/wallets/{division}/transactions/
        /// </summary>
        /// <param name="division"></param>
        /// <param name="from_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Transaction>>> CorporationTransactions(int division, long from_id,
            string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Transaction>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/wallets/{division}/transactions/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"corporation_id", corporation_id.ToString()},
                    {"division", division.ToString()}
                },
                parameters: new string[]
                {
                    $"from_id={from_id}"
                },
                token: _data.Token);
    }
}