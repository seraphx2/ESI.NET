using ESI.NET.Models.Contracts;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class ContractsLogic : _BaseLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly AuthorizedCharacterData _data;
        private readonly int character_id, corporation_id;

        public ContractsLogic(HttpClient client, EsiConfig config, AuthorizedCharacterData data = null)
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
        public async Task<EsiResponse<List<Contract>>> CharacterContracts()
            => await Execute<List<Contract>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contracts/", token: _data.Token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<ContractItem>>> CharacterContractItems(int contract_id)
            => await Execute<List<ContractItem>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contracts/{contract_id}/items/", token: _data.Token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Bid>>> CharacterContractBids(int contract_id)
            => await Execute<List<Bid>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contracts/{contract_id}/bids/", token: _data.Token);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Contract>>> CorporationContracts()
            => await Execute<List<Contract>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/contracts/", token: _data.Token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<ContractItem>>> CorporationContractItems(int contract_id)
            => await Execute<List<ContractItem>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/contracts/{contract_id}/items/", token: _data.Token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Bid>>> CorporationContractBids(int contract_id)
            => await Execute<List<Bid>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/contracts/{contract_id}/bids/", token: _data.Token);
    }
}