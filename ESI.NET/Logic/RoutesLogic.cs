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
    public class RoutesLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public RoutesLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>
        /// POST /route/{origin_system_id}/{destination_system_id}/
        /// </summary>
        /// <param name="originSystemId">Origin solar system id</param>
        /// <param name="destinationSystemId">Destination solar system id</param>
        /// <param name="flag">Routing preference</param>
        /// <param name="avoidSystems">Solar system ids to avoid</param>
        /// <param name="connections">System-id pairs (<c>[from, to]</c>) to treat as connected</param>
        /// <param name="securityPenalty">Penalty applied per low/null-sec jump (server default 50)</param>
        public async Task<EsiResponse<RouteResult>> Map(
            long originSystemId,
            long destinationSystemId,
            RoutesFlag flag = RoutesFlag.Shorter,
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
