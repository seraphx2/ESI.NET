using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Contracts;

namespace ESI.NET.Logic.Interfaces
{
    public interface IContractsLogic
    {
        Task<ApiResponse<List<Bid>>> CharacterContractBids(int contract_id);
        Task<ApiResponse<List<ContractItem>>> CharacterContractItems(int contract_id);
        Task<ApiResponse<List<Contract>>> CharacterContracts();
        Task<ApiResponse<List<Bid>>> CorporationContractBids(int contract_id);
        Task<ApiResponse<List<ContractItem>>> CorporationContractItems(int contract_id);
        Task<ApiResponse<List<Contract>>> CorporationContracts();
    }
}