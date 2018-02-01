using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Wars;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class WarsLogic : IWarsLogic
    {
        private ESIConfig _config;
        public WarsLogic(ESIConfig config) { _config = config; }

        /// <summary>
        /// /wars/
        /// </summary>
        /// <param name="max_war_id">Only return wars with ID smaller than this</param>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> All(long max_war_id = 0)
        {
            var parameters = new List<string>();

            if (max_war_id > 0)
                parameters.Add($"max_war_id={max_war_id}");

            var endpoint = "/wars/";
            var response = await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint, parameters.ToArray());

            return response;
        }

        /// <summary>
        /// /wars/{warId}/
        /// </summary>
        /// <param name="war_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Information>> Information(int war_id)
        {
            var endpoint = $"/wars/{war_id}/";
            var response = await Execute<Information>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /wars/{warId}/killmails/
        /// </summary>
        /// <param name="war_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Models.Killmails.Killmail>>> Kills(int war_id, int page = 1)
        {
            var endpoint = $"/wars/{war_id}/killmails/";
            var response = await Execute<List<Models.Killmails.Killmail>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint, new string[]
            {
                $"page={page}"
            });

            return response;
        }
    }
}
