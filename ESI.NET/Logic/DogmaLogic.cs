using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Dogma;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class DogmaLogic : IDogmaLogic
    {
        private ESIConfig _config;

        public DogmaLogic(ESIConfig config) { _config = config; }

        /// <summary>
        /// /dogma/attributes/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Attributes()
            => await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, "/dogma/attributes/");

        /// <summary>
        /// /dogma/attributes/{attribute_id}/
        /// </summary>
        /// <param name="attribute_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Attribute>> Attribute(int attribute_id)
            => await Execute<Attribute>(_config, RequestSecurity.Public, RequestMethod.GET, $"/dogma/attributes/{attribute_id}/");

        /// <summary>
        /// /dogma/effects/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Effects()
            => await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, "/dogma/effects/");

        /// <summary>
        /// /dogma/effects/{effect_id}/
        /// </summary>
        /// <param name="effect_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Effect>> Effect(int effect_id)
            => await Execute<Effect>(_config, RequestSecurity.Public, RequestMethod.GET, $"/dogma/effects/{effect_id}/");
    }
}