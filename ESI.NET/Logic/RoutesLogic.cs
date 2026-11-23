using ESI.NET.Enumerations;
using ESI.NET.Models.Routes;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Routes endpoints. Reach them through <see cref="EsiClient.Routes"/>.
    /// </summary>
    public class RoutesLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Routes endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public RoutesLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>
        /// Get route between two systems. Calculate the systems between the given origin and
        /// destination.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>POST /route/{origin_system_id}/{destination_system_id}</c>.</para>
        /// </remarks>
        /// <param name="originSystemId">Origin system.</param>
        /// <param name="destinationSystemId">Destination system.</param>
        /// <param name="flag">Which route to prefer:
        /// <see cref="ESI.NET.Enumerations.RoutePreference.Shorter"/>,
        /// <see cref="ESI.NET.Enumerations.RoutePreference.Safer"/> (avoids low and null sec), or
        /// <see cref="ESI.NET.Enumerations.RoutePreference.LessSecure"/> (prefers them).</param>
        /// <param name="avoidSystems">Systems to avoid.</param>
        /// <param name="connections">Additional one-way connections (like Jump Bridges) between
        /// systems.</param>
        /// <param name="securityPenalty">Strictness of the path preference.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="RouteResult"/>.</returns>
        public async Task<EsiResponse<RouteResult>> Map(
            long originSystemId,
            long destinationSystemId,
            RoutePreference flag = RoutePreference.Shorter,
            long[] avoidSystems = null,
            long[][] connections = null,
            int? securityPenalty = null,
            EsiCallOptions options = null)
        {
            var payload = new Dictionary<string, object> { ["preference"] = flag.ToEsiValue() };

            if (avoidSystems != null)
                payload["avoid_systems"] = avoidSystems;
            if (connections != null)
                payload["connections"] = connections.Select(c => new { from = c[0], to = c[1] });
            if (securityPenalty.HasValue)
                payload["security_penalty"] = securityPenalty.Value;

            return await Execute<RouteResult>(_client, _config, RequestSecurity.Public, HttpMethod.Post,
                "/route/{origin_system_id}/{destination_system_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "origin_system_id", originSystemId.ToString(CultureInfo.InvariantCulture) },
                    { "destination_system_id", destinationSystemId.ToString(CultureInfo.InvariantCulture) }
                },
                body: payload,
                options: options).ConfigureAwait(false);
        }
    }
}
