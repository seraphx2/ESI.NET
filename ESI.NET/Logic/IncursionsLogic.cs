using ESI.NET.Models.Incursions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class IncursionsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public IncursionsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /incursions/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Incursion>>> All(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Incursion>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/incursions/",
                eTag: eTag,
                cancellationToken: cancellationToken);
    }
}