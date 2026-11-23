using ESI.NET.Models.Fittings;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Fittings endpoints. Reach them through <see cref="EsiClient.Fittings"/>.
    /// </summary>
    public class FittingsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Fittings endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public FittingsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>Return fittings of a character.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/fittings</c>.</para>
        /// <para>Requires the <c>esi-fittings.read_fittings.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Fitting"/>.</returns>
        public async Task<EsiResponse<List<Fitting>>> List(EsiCallOptions options)
            => await Execute<List<Fitting>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/fittings/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Create fitting. Save a new fitting for a character.</summary>
        /// <remarks>
        /// <para>Calls <c>POST /characters/{character_id}/fittings</c>.</para>
        /// <para>Requires the <c>esi-fittings.write_fittings.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fitting">The fitting to save: its name, description, ship type ID and items
        /// (see <see cref="ESI.NET.Models.Fittings.Fitting"/>).</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="NewFitting"/>.</returns>
        public async Task<EsiResponse<NewFitting>> Add(object fitting, EsiCallOptions options)
            => await Execute<NewFitting>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/characters/{character_id}/fittings/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                body: fitting,
                options: options).ConfigureAwait(false);

        /// <summary>Delete a fitting from a character.</summary>
        /// <remarks>
        /// <para>Calls <c>DELETE /characters/{character_id}/fittings/{fitting_id}</c>.</para>
        /// <para>Requires the <c>esi-fittings.write_fittings.v1</c> scope.</para>
        /// </remarks>
        /// <param name="fittingId">ID for a fitting of this character.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> Delete(long fittingId, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Delete, "/characters/{character_id}/fittings/{fitting_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "fitting_id", fittingId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}