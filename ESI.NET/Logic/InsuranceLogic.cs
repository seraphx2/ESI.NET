using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Insurance;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class InsuranceLogic : IInsuranceLogic
    {
        private ESIConfig _config;

        public InsuranceLogic(ESIConfig config) { _config = config; }

        /// <summary>
        /// /insurance/prices/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Insurance>>> Levels()
            => await Execute<List<Insurance>>(_config, RequestSecurity.Public, RequestMethod.GET, "/insurance/prices/");
    }
}