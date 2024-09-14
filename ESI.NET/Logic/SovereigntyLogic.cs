using ESI.NET.Models.Sovereignty;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Interfaces.Logic;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class SovereigntyLogic : ISovereigntyLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public SovereigntyLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /sovereignty/campaigns/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Campaign>>> Campaigns(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Campaign>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/sovereignty/campaigns/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /sovereignty/map/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<SystemSovereignty>>> Systems(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<SystemSovereignty>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/sovereignty/map/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /sovereignty/structures/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Structure>>> Structures(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Structure>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/sovereignty/structures/",
                eTag: eTag,
                cancellationToken: cancellationToken);
    }
}