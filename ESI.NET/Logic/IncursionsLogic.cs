using ESI.NET.Models.Incursions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class IncursionsLogic
    {
        private HttpClient _client;
        private ESIConfig _config;

        public IncursionsLogic(HttpClient client, ESIConfig config) { _client = client; _config = config; }

        /// <summary>
        /// /incursions/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Incursion>>> All()
            => await Execute<List<Incursion>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/incursions/");
    }
}