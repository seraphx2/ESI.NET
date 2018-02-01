using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Assets;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class AssetsLogic : IAssetsLogic
    {
        private ESIConfig _config;
        private int character_id, corporation_id;

        public AssetsLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                character_id = _config.AuthorizedCharacter.CharacterID;
                corporation_id = _config.AuthorizedCharacter.CorporationID;
            }
        }

        /// <summary>
        /// /characters/{character_id}/assets/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Item>>> ForCharacter(int page = 1)
        {
            var endpoint = $"/characters/{character_id}/assets/";
            var response = await Execute<List<Item>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/assets/locations/
        /// </summary>
        /// <param name="item_ids"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ItemLocation>>> LocationsForCharacter(List<long> item_ids)
        {
            var endpoint = $"/characters/{character_id}/assets/locations/";
            var response = await Execute<List<ItemLocation>>(_config, RequestSecurity.Authenticated, RequestMethod.POST, endpoint, body: item_ids.ToArray());

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/assets/names/
        /// </summary>
        /// <param name="item_ids"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ItemName>>> NamesForCharacter(List<long> item_ids)
        {
            var endpoint = $"/characters/{character_id}/assets/names/";
            var response = await Execute<List<ItemName>>(_config, RequestSecurity.Authenticated, RequestMethod.POST, endpoint, body: item_ids.ToArray());

            return response;
        }


        /// <summary>
        /// /corporations/{corporation_id}/assets/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Item>>> ForCorporation(int page = 1)
        {
            var endpoint = $"/corporations/{corporation_id}/assets/";
            var response = await Execute<List<Item>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }

        /// <summary>
        /// /corporations/{corporation_id}/assets/locations/
        /// </summary>
        /// <param name="item_ids"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ItemLocation>>> LocationsForCorporation(List<long> item_ids)
        {
            var endpoint = $"/corporations/{corporation_id}/assets/locations/";
            var response = await Execute<List<ItemLocation>>(_config, RequestSecurity.Authenticated, RequestMethod.POST, endpoint, body: item_ids.ToArray());

            return response;
        }

        /// <summary>
        /// /corporations/{corporation_id}/assets/names/
        /// </summary>
        /// <param name="item_ids"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ItemName>>> NamesForCorporation(List<long> item_ids)
        {
            var endpoint = $"/corporations/{corporation_id}/assets/names/";
            var response = await Execute<List<ItemName>>(_config, RequestSecurity.Authenticated, RequestMethod.POST, endpoint, body: item_ids.ToArray());

            return response;
        }
    }
}
