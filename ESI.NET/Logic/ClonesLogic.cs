using ESI.NET.Models.Clones;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Clones endpoints. Reach them through <see cref="EsiClient.Clones"/>.
    /// </summary>
    public class ClonesLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Clones endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public ClonesLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>A list of the character's clones.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/clones</c>.</para>
        /// <para>Requires the <c>esi-clones.read_clones.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Clones"/>.</returns>
        public async Task<EsiResponse<Clones>> List(EsiCallOptions options)
            => await Execute<Clones>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/clones/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get active implants. Return implants on the active clone of a character.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/implants</c>.</para>
        /// <para>Requires the <c>esi-clones.read_implants.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <c>long</c> values.</returns>
        public async Task<EsiResponse<long[]>> Implants(EsiCallOptions options)
            => await Execute<long[]>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/implants/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}