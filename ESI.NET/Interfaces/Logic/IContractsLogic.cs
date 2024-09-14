using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Contracts;

namespace ESI.NET.Interfaces.Logic
{
    public interface IContractsLogic
    {
        /// <summary>
        /// /contracts/public/{region_id}/
        /// </summary>
        /// <param name="region_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Contract>>> Contracts(int region_id, int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /contracts/public/items/{contract_id}/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<ContractItem>>> ContractItems(int contract_id, int page = 1,
            string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// "/contracts/public/bids/{contract_id}/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Bid>>> ContractBids(int contract_id, int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/contracts/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Contract>>> CharacterContracts(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/contracts/{contract_id}/items/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<ContractItem>>> CharacterContractItems(int contract_id, int page = 1,
            string eTag = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/contracts/{contract_id}/bids/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Bid>>> CharacterContractBids(int contract_id, int page = 1,
            string eTag = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/contracts/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Contract>>> CorporationContracts(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/contracts/{contract_id}/items/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<ContractItem>>> CorporationContractItems(int contract_id, int page = 1,
            string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/contracts/{contract_id}/bids/
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Bid>>> CorporationContractBids(int contract_id, int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);
    }
}