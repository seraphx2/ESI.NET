using ESI.NET.Enumerations;
using ESI.NET.Models.Routes;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class RoutesLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public RoutesLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>
        /// POST /route/{origin_system_id}/{destination_system_id}/
        /// </summary>
        /// <param name="origin_system_id">Origin solar system id</param>
        /// <param name="destination_system_id">Destination solar system id</param>
        /// <param name="flag">Routing preference</param>
        /// <param name="avoid_systems">Solar system ids to avoid</param>
        /// <param name="connections">System-id pairs (<c>[from, to]</c>) to treat as connected</param>
        /// <param name="security_penalty">Penalty applied per low/null-sec jump (server default 50)</param>
        public async Task<EsiResponse<RouteResult>> Map(
            long origin_system_id,
            long destination_system_id,
            RoutesFlag flag = RoutesFlag.Shorter,
            long[] avoid_systems = null,
            long[][] connections = null,
            int? security_penalty = null,
            EsiCallOptions options = null)
        {
            var payload = new Dictionary<string, object> { ["preference"] = flag.ToEsiValue() };

            if (avoid_systems != null)
                payload["avoid_systems"] = avoid_systems;
            if (connections != null)
                payload["connections"] = connections.Select(c => new { from = c[0], to = c[1] });
            if (security_penalty.HasValue)
                payload["security_penalty"] = security_penalty.Value;

            return await Execute<RouteResult>(_client, _config, RequestSecurity.Public, HttpMethod.Post,
                "/route/{origin_system_id}/{destination_system_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "origin_system_id", origin_system_id.ToString() },
                    { "destination_system_id", destination_system_id.ToString() }
                },
                body: payload,
                options: options).ConfigureAwait(false);
        }
    }
}
