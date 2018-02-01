using ESI.NET.Enumerations;
using ESI.NET.Logic.Interfaces;
using ESI.NET.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class RoutesLogic : IRoutesLogic
    {
        private ESIConfig _config;

        public RoutesLogic(ESIConfig config) { _config = config; }

        /// <summary>
        /// /route/{origin}/{destination}/
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <param name="flag"></param>
        /// <param name="avoid"></param>
        /// <param name="connections"></param>
        /// <returns></returns>
        public async Task<ApiResponse<int[]>> Map(
            int origin, 
            int destination, 
            RoutesFlag flag = RoutesFlag.shortest, 
            int[] avoid = null, 
            int[] connections = null)
        {
            var parameters = new List<string>() { $"flag={flag}" };

            if (avoid != null)
                parameters.Add($"&avoid={string.Join(",", avoid)}");

            if (connections != null)
                parameters.Add($"&connections={string.Join(",", connections)}");

            var endpoint = $"/route/{origin}/{destination}/";
            var response = await Execute<int[]>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint, parameters.ToArray());

            return response;
        }
    }
}
