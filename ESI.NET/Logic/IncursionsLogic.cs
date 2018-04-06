using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Incursions;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class IncursionsLogic : IIncursionsLogic
    {
        private ESIConfig _config;

        public IncursionsLogic(ESIConfig config) { _config = config; }

        /// <summary>
        /// /incursions/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Incursion>>> All()
            => await Execute<List<Incursion>>(_config, RequestSecurity.Public, RequestMethod.GET, "/incursions/");
    }
}