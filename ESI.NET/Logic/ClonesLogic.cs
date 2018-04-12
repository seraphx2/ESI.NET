using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Clones;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class ClonesLogic : IClones
    {
        private ESIConfig _config;
        private int character_id;

        public ClonesLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
                character_id = _config.AuthorizedCharacter.CharacterID;
        }

        /// <summary>
        /// /characters/{character_id}/clones/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<Clones>> List()
            => await Execute<Clones>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/clones/");

        /// <summary>
        /// /characters/{character_id}/implants/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<int[]>> Implants()
            => await Execute<int[]>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/implants/");
    }
}