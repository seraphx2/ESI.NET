using ESI.NET.Models.Insurance;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Interfaces.Logic;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class InsuranceLogic : IInsuranceLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public InsuranceLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /insurance/prices/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Insurance>>> Levels(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Insurance>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/insurance/prices/",
                eTag: eTag,
                cancellationToken: cancellationToken);
    }
}