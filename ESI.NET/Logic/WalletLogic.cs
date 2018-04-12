using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class WalletLogic : IWalletLogic
    {
        private ESIConfig _config;
        private int character_id, corporation_id;

        public WalletLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                character_id = _config.AuthorizedCharacter.CharacterID;
                corporation_id = config.AuthorizedCharacter.CorporationID;
            }
                
        }

        /// <summary>
        /// /characters/{character_id}/wallet/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<decimal>> CharacterWallet()
            => await Execute<decimal>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/wallet/");

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

            return await Execute<List<JournalEntry>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/wallet/journal/", parameters.ToArray());
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

            return await Execute<List<Transaction>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/wallet/transactions/", parameters.ToArray());
        }

        /// <summary>
        /// /corporations/{corporation_id}/wallets/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Wallet>>> CorporationWallets()
            => await Execute<List<Wallet>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/wallets/");

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

            return await Execute<List<JournalEntry>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/wallets/{division}/journal/", parameters.ToArray());
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

            return await Execute<List<Transaction>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/wallets/{division}/transactions/", parameters.ToArray());
        }
    }
}