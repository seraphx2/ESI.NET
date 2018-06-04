using ESI.NET.Models.Fittings;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class FittingsLogic
    {
        private HttpClient _client;
        private ESIConfig _config;
        private AuthorizedCharacterData _data;
        private int character_id;

        public FittingsLogic(HttpClient client, ESIConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
                character_id = data.CharacterID;
        }

        /// <summary>
        /// /characters/{character_id}/fittings/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Fitting>>> List()
            => await Execute<List<Fitting>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/fittings/, token: _data.Token");

        /// <summary>
        /// /characters/{character_id}/fittings/
        /// </summary>
        /// <param name="fitting"></param>
        /// <returns></returns>
        public async Task<ApiResponse<NewFitting>> Add(object fitting)
            => await Execute<NewFitting>(_client, _config, RequestSecurity.Authenticated, RequestMethod.POST, $"/characters/{character_id}/fittings/", body: fitting, token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/fittings/{fitting_id}/
        /// </summary>
        /// <param name="fitting_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> Delete(int fitting_id)
        {
            var response = await Execute<string>(_client, _config, RequestSecurity.Authenticated, RequestMethod.DELETE, $"/characters/{character_id}/fittings/{fitting_id}/", token: _data.Token);

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["DELETE|/characters/{character_id}/fittings/{fitting_id}/"];

            return response;
        }
    }
}