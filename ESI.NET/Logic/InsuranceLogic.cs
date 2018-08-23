using ESI.NET.Models.Insurance;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class InsuranceLogic
    {
        private HttpClient _client;
        private ESIConfig _config;

        public InsuranceLogic(HttpClient client, ESIConfig config) { _client = client; _config = config; }

        /// <summary>
        /// /insurance/prices/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Insurance>>> Levels()
            => await Execute<List<Insurance>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/insurance/prices/");
    }
}