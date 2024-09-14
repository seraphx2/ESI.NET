using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Wallet;

namespace ESI.NET.Interfaces.Logic
{
    public interface IWalletLogic
    {
        /// <summary>
        /// /characters/{character_id}/wallet/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<decimal>> CharacterWallet(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/wallet/journal/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<JournalEntry>>> CharacterJournal(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/wallet/transactions/
        /// </summary>
        /// <param name="from_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Transaction>>> CharacterTransactions(long from_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/wallets/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Wallet>>> CorporationWallets(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/wallets/{division}/journal/
        /// </summary>
        /// <param name="division"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<JournalEntry>>> CorporationJournal(int division, int page = 1,
            string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/wallets/{division}/transactions/
        /// </summary>
        /// <param name="division"></param>
        /// <param name="from_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Transaction>>> CorporationTransactions(int division, long from_id,
            string eTag = null,
            CancellationToken cancellationToken = default);
    }
}