using ESI.NET.Models.Dogma;
using ESI.NET.Models.Opportunities;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class DogmaLogic
    {
        private ESIConfig _config;

        public DogmaLogic(ESIConfig config) { _config = config; }

        /// <summary>
        /// /dogma/attributes/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Attributes()
        {
            var endpoint = "/dogma/attributes/";
            var response = await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /dogma/attributes/{attribute_id}/
        /// </summary>
        /// <param name="attribute_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Attribute>> Attribute(int attribute_id)
        {
            var endpoint = $"/dogma/attributes/{attribute_id}/";
            var response = await Execute<Attribute>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /dogma/effects/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Effects()
        {
            var endpoint = "/dogma/effects/";
            var response = await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /dogma/effects/{effect_id}/
        /// </summary>
        /// <param name="effect_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Effect>> Effect(int effect_id)
        {
            var endpoint = $"/dogma/effects/{effect_id}/";
            var response = await Execute<Effect>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }
    }
}
