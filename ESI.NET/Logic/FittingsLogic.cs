using ESI.NET.Logic.Interface;
using ESI.NET.Models.Fittings;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class FittingsLogic : IFittingsLogic
    {
        private ESIConfig _config;
        private int character_id;

        public FittingsLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
                character_id = _config.AuthorizedCharacter.CharacterID;
        }

        /// <summary>
        /// /characters/{character_id}/fittings/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Fitting>>> List()
            => await Execute<List<Fitting>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/fittings/");

        /// <summary>
        /// /characters/{character_id}/fittings/
        /// </summary>
        /// <param name="fitting"></param>
        /// <returns></returns>
        public async Task<ApiResponse<NewFitting>> Add(object fitting)
            => await Execute<NewFitting>(_config, RequestSecurity.Authenticated, RequestMethod.POST, $"/characters/{character_id}/fittings/", body: fitting);

        /// <summary>
        /// /characters/{character_id}/fittings/{fitting_id}/
        /// </summary>
        /// <param name="fitting_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> Delete(int fitting_id)
        {
            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.DELETE, $"/characters/{character_id}/fittings/{fitting_id}/");

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["DELETE|/characters/{character_id}/fittings/{fitting_id}/"];

            return response;
        }
    }
}