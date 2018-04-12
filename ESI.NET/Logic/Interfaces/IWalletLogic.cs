using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Wallet;

namespace ESI.NET.Logic.Interfaces
{
    public interface IWalletLogic
    {
        Task<ApiResponse<List<JournalEntry>>> CharacterJournal(long? from_id = null);
        Task<ApiResponse<List<Transaction>>> CharacterTransactions(long? from_id = null);
        Task<ApiResponse<decimal>> CharacterWallet();
        Task<ApiResponse<List<JournalEntry>>> CorporationJournal(int division, long? from_id = null);
        Task<ApiResponse<List<Transaction>>> CorporationTransactions(int division, long? from_id = null);
        Task<ApiResponse<List<Wallet>>> CorporationWallets();
    }
}