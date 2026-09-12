using ESI.NET.Models.Dogma;
using System.Collections.Generic;
using System.Globalization;
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
        public async Task<EsiResponse<long[]>> Attributes(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/dogma/attributes/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /dogma/attributes/{attribute_id}/
        /// </summary>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<AttributeInfo>> Attribute(long attributeId, EsiCallOptions options = null)
            => await Execute<AttributeInfo>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/dogma/attributes/{attribute_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "attribute_id", attributeId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /dogma/effects/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Effects(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/dogma/effects/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /dogma/effects/{effect_id}/
        /// </summary>
        /// <param name="effectId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<EffectInfo>> Effect(long effectId, EsiCallOptions options = null)
            => await Execute<EffectInfo>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/dogma/effects/{effect_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "effect_id", effectId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /dogma/dynamic/items/{type_id}/{item_id}/
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<DynamicItem>> DynamicItem(long typeId, long itemId, EsiCallOptions options = null)
            => await Execute<DynamicItem>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/dogma/dynamic/items/{type_id}/{item_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "type_id", typeId.ToString(CultureInfo.InvariantCulture) },
                    { "item_id", itemId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

    }
}