using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Sovereignty;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class SovereigntyLogic : ISovereigntyLogic
    {
        private ESIConfig _config;

        public SovereigntyLogic(ESIConfig config) { _config = config; }

        /// <summary>
        /// /sovereignty/campaigns/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Campaign>>> Campaigns()
            => await Execute<List<Campaign>>(_config, RequestSecurity.Public, RequestMethod.GET, "/sovereignty/campaigns/");

        /// <summary>
        /// /sovereignty/map/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<SystemSovereignty>>> Systems()
            => await Execute<List<SystemSovereignty>>(_config, RequestSecurity.Public, RequestMethod.GET, "/sovereignty/map/");

        /// <summary>
        /// /sovereignty/structures/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Structure>>> Structures()
            => await Execute<List<Structure>>(_config, RequestSecurity.Public, RequestMethod.GET, "/sovereignty/structures/");
    }
}