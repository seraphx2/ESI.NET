using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class ContractsLogic : IContractsLogic
    {
        private ESIConfig _config;
        private int character_id, corporation_id;

        public ContractsLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                character_id = _config.AuthorizedCharacter.CharacterID;
                corporation_id = config.AuthorizedCharacter.CorporationID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Contract>>> CharacterContracts()
            => await Execute<List<Contract>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contracts/");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ContractItem>>> CharacterContractItems(int contract_id)
            => await Execute<List<ContractItem>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contracts/{contract_id}/items/");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bid>>> CharacterContractBids(int contract_id)
            => await Execute<List<Bid>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contracts/{contract_id}/bids/");

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Contract>>> CorporationContracts()
            => await Execute<List<Contract>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/contracts/");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ContractItem>>> CorporationContractItems(int contract_id)
            => await Execute<List<ContractItem>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/contracts/{contract_id}/items/");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bid>>> CorporationContractBids(int contract_id)
            => await Execute<List<Bid>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/contracts/{contract_id}/bids/");
    }
}