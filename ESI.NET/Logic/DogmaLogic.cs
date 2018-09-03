using ESI.NET.Models.Dogma;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class DogmaLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public DogmaLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>
        /// /dogma/attributes/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<int>>> Attributes()
            => await Execute<List<int>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/dogma/attributes/");

        /// <summary>
        /// /dogma/attributes/{attribute_id}/
        /// </summary>
        /// <param name="attribute_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Attribute>> Attribute(int attribute_id)
            => await Execute<Attribute>(_client, _config, RequestSecurity.Public, RequestMethod.GET, $"/dogma/attributes/{attribute_id}/");

        /// <summary>
        /// /dogma/effects/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<int>>> Effects()
            => await Execute<List<int>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/dogma/effects/");

        /// <summary>
        /// /dogma/effects/{effect_id}/
        /// </summary>
        /// <param name="effect_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Effect>> Effect(int effect_id)
            => await Execute<Effect>(_client, _config, RequestSecurity.Public, RequestMethod.GET, $"/dogma/effects/{effect_id}/");

        /// <summary>
        /// /dogma/dynamic/items/{type_id}/{item_id}/
        /// </summary>
        /// <param name="type_id"></param>
        /// <param name="item_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Effect>> DynamicItem(int type_id, int item_id)
            => await Execute<Effect>(_client, _config, RequestSecurity.Public, RequestMethod.GET, $"/dogma/dynamic/items/{type_id}/{item_id}/");
    }
}