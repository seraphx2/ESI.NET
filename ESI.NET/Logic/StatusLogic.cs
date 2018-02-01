using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Status;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class StatusLogic : IStatusLogic
    {
        private ESIConfig _config;

        public StatusLogic(ESIConfig config) { _config = config; }

        public async Task<ApiResponse<Status>> Retrieve()
        {
            var url = "/status/";
            var response = await Execute<Status>(_config, RequestSecurity.Public, RequestMethod.GET, url);

            return response;
        }
    }
}
