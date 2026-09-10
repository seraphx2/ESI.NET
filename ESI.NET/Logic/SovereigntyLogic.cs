using ESI.NET.Models.Sovereignty;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class SovereigntyLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public SovereigntyLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>
        /// /sovereignty/campaigns/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Campaign>>> Campaigns(EsiCallOptions options = null)
            => await Execute<List<Campaign>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/sovereignty/campaigns/",
                options: options);


        /// <summary>
        /// /sovereignty/map/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<SystemSovereignty>>> Systems(EsiCallOptions options = null)
            => await Execute<List<SystemSovereignty>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/sovereignty/map/",
                options: options);


        /// <summary>
        /// /sovereignty/structures/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Structure>>> Structures(EsiCallOptions options = null)
            => await Execute<List<Structure>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/sovereignty/structures/",
                options: options);

    }
}