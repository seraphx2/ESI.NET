using ESI.NET.Models.Status;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Interfaces.Logic;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class StatusLogic : IStatusLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public StatusLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        public async Task<EsiResponse<Status>> Retrieve(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Status>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/status/",
                eTag: eTag,
                cancellationToken: cancellationToken);
    }
}