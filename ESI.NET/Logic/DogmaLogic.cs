using ESI.NET.Models.Dogma;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Dogma endpoints. Reach them through <see cref="EsiClient.Dogma"/>.
    /// </summary>
    public class DogmaLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Dogma endpoints. <see cref="EsiClient"/> does this; there is no need to call
        /// it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public DogmaLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>
        /// Get attributes. Get a list of dogma attribute ids This route expires daily at 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /dogma/attributes</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <c>long</c> values.</returns>
        public async Task<EsiResponse<long[]>> Attributes(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/dogma/attributes/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Get attribute information. Get information on a dogma attribute This route expires daily
        /// at 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /dogma/attributes/{attribute_id}</c>.</para>
        /// </remarks>
        /// <param name="attributeId">A dogma attribute ID.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="AttributeInfo"/>.</returns>
        public async Task<EsiResponse<AttributeInfo>> Attribute(long attributeId, EsiCallOptions options = null)
            => await Execute<AttributeInfo>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/dogma/attributes/{attribute_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "attribute_id", attributeId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Get effects. Get a list of dogma effect ids This route expires daily at 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /dogma/effects</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <c>long</c> values.</returns>
        public async Task<EsiResponse<long[]>> Effects(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/dogma/effects/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Get effect information. Get information on a dogma effect This route expires daily at
        /// 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /dogma/effects/{effect_id}</c>.</para>
        /// </remarks>
        /// <param name="effectId">A dogma effect ID.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="EffectInfo"/>.</returns>
        public async Task<EsiResponse<EffectInfo>> Effect(long effectId, EsiCallOptions options = null)
            => await Execute<EffectInfo>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/dogma/effects/{effect_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "effect_id", effectId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Get dynamic item information. Returns info about a dynamic item resulting from mutation
        /// with a mutaplasmid. This route expires daily at 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /dogma/dynamic/items/{type_id}/{item_id}</c>.</para>
        /// </remarks>
        /// <param name="typeId">Type ID of the mutated (abyssal) item.</param>
        /// <param name="itemId">Item ID of the mutated item.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="DynamicItem"/>.</returns>
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