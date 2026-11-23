using ESI.NET.Models.Wars;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Wars endpoints. Reach them through <see cref="EsiClient.Wars"/>.
    /// </summary>
    public class WarsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        /// <summary>
        /// Creates the Wars endpoints. <see cref="EsiClient"/> does this; there is no need to call
        /// it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public WarsLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>Return a list of wars.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /wars</c>.</para>
        /// </remarks>
        /// <param name="maxWarId">Only return wars with ID smaller than this.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <c>long</c> values.</returns>
        public async Task<EsiResponse<long[]>> All(long maxWarId = 0, EsiCallOptions options = null)
        {
            var parameters = new List<string>();

            if (maxWarId > 0)
                parameters.Add($"max_war_id={maxWarId}");

            var response = await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/wars/",
                parameters: parameters.ToArray(),
                options: options).ConfigureAwait(false);

            return response;
        }

        /// <summary>Get war information. Return details about a war.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /wars/{war_id}</c>.</para>
        /// </remarks>
        /// <param name="warId">ID for a war.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="War"/>.</returns>
        public async Task<EsiResponse<War>> Information(long warId, EsiCallOptions options = null)
            => await Execute<War>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/wars/{war_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "war_id", warId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>Return a list of kills related to a war.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /wars/{war_id}/killmails</c>.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="warId">A valid war ID.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>, <see cref="EsiCallOptions.BypassCache"/>, and
        /// <see cref="EsiCallOptions.Page"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Models.Killmails.Killmail"/>.</returns>
        public async Task<EsiResponse<List<Models.Killmails.Killmail>>> Kills(long warId, EsiCallOptions options = null)
            => await Execute<List<Models.Killmails.Killmail>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/wars/{war_id}/killmails/",
                replacements: new Dictionary<string, string>()
                {
                    { "war_id", warId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}