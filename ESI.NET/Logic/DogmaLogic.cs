using ESI.NET.Models.Dogma;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Interfaces.Logic;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class DogmaLogic : IDogmaLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public DogmaLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /dogma/attributes/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Attributes(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/dogma/attributes/",
                eTag: eTag, cancellationToken: cancellationToken);

        /// <summary>
        /// /dogma/attributes/{attribute_id}/
        /// </summary>
        /// <param name="attribute_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Attribute>> Attribute(int attribute_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Attribute>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/dogma/attributes/{attribute_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {
                        "attribute_id", attribute_id.ToString()
                    }
                });

        /// <summary>
        /// /dogma/effects/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Effects(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/dogma/effects/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /dogma/effects/{effect_id}/
        /// </summary>
        /// <param name="effect_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Effect>> Effect(int effect_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Effect>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/dogma/effects/{effect_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"effect_id", effect_id.ToString()}
                });

        /// <summary>
        /// /dogma/dynamic/items/{type_id}/{item_id}/
        /// </summary>
        /// <param name="type_id"></param>
        /// <param name="item_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Effect>> DynamicItem(int type_id, long item_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Effect>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/dogma/dynamic/items/{type_id}/{item_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"type_id", type_id.ToString()},
                    {"item_id", item_id.ToString()}
                });
    }
}