using ESI.NET.Models.SSO;
using ESI.NET.Models.Wallet;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class WalletLogic
    {
        private HttpClient _client;
        private ESIConfig _config;
        private AuthorizedCharacterData _data;
        private int character_id, corporation_id;

        public WalletLogic(HttpClient client, ESIConfig config, AuthorizedCharacterData data = null)
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
        public async Task<ApiResponse<decimal>> CharacterWallet()
            => await Execute<decimal>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/wallet/", token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/wallet/journal/
        /// </summary>
        /// <param name="from_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<JournalEntry>>> CharacterJournal(long? from_id = null)
        {
            var parameters = new List<string>();
            if (from_id != null)
                parameters.Add($"from_id={from_id}");

            return await Execute<List<JournalEntry>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/wallet/journal/", parameters.ToArray(), token: _data.Token);
        }
            

        /// <summary>
        /// /characters/{character_id}/wallet/transactions/
        /// </summary>
        /// <param name="from_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Transaction>>> CharacterTransactions(long? from_id = null)
        {
            var parameters = new List<string>();
            if (from_id != null)
                parameters.Add($"from_id={from_id}");

            return await Execute<List<Transaction>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/wallet/transactions/", parameters.ToArray(), token: _data.Token);
        }

        /// <summary>
        /// /corporations/{corporation_id}/wallets/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Wallet>>> CorporationWallets()
            => await Execute<List<Wallet>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/wallets/", token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/wallets/{division}/journal/
        /// </summary>
        /// <param name="division"></param>
        /// <param name="from_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<JournalEntry>>> CorporationJournal(int division, long? from_id = null)
        {
            var parameters = new List<string>();
            if (from_id != null)
                parameters.Add($"from_id={from_id}");

            return await Execute<List<JournalEntry>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/wallets/{division}/journal/", parameters.ToArray(), token: _data.Token);
        }

        /// <summary>
        /// /corporations/{corporation_id}/wallets/{division}/transactions/
        /// </summary>
        /// <param name="division"></param>
        /// <param name="from_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Transaction>>> CorporationTransactions(int division, long? from_id = null)
        {
            var parameters = new List<string>();
            if (from_id != null)
                parameters.Add($"from_id={from_id}");

            return await Execute<List<Transaction>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/wallets/{division}/transactions/", parameters.ToArray(), token: _data.Token);
        }
    }
}