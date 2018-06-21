using ESI.NET.Models.Contracts;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class ContractsLogic
    {
        private HttpClient _client;
        private ESIConfig _config;
        private AuthorizedCharacterData _data;
        private int character_id, corporation_id;

        public ContractsLogic(HttpClient client, ESIConfig config, AuthorizedCharacterData data = null)
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
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Contract>>> CharacterContracts()
            => await Execute<List<Contract>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contracts/", token: _data.Token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ContractItem>>> CharacterContractItems(int contract_id)
            => await Execute<List<ContractItem>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contracts/{contract_id}/items/", token: _data.Token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bid>>> CharacterContractBids(int contract_id)
            => await Execute<List<Bid>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contracts/{contract_id}/bids/", token: _data.Token);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Contract>>> CorporationContracts()
            => await Execute<List<Contract>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/contracts/", token: _data.Token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ContractItem>>> CorporationContractItems(int contract_id)
            => await Execute<List<ContractItem>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/contracts/{contract_id}/items/", token: _data.Token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bid>>> CorporationContractBids(int contract_id)
            => await Execute<List<Bid>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/contracts/{contract_id}/bids/", token: _data.Token);
    }
}