using ESI.NET.Logic.Interfaces;
using ESI.NET.Models;
using ESI.NET.Models.Alliance;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class AllianceLogic : IAllianceLogic
    {
        private ESIConfig _config;

        public AllianceLogic(ESIConfig config) { _config = config; }

        /// <summary>
        /// /alliances/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> All()
            => await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, "/alliances/");

        /// <summary>
        /// /alliances/names/
        /// </summary>
        /// <param name="alliance_ids"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Alliance>>> Names(List<int> alliance_ids)
            => await Execute<List<Alliance>>(_config, RequestSecurity.Public, RequestMethod.GET, "/alliances/names/", new string[]
            {
                $"alliance_ids={string.Join(",", alliance_ids)}"
            });

        /// <summary>
        /// /alliances/{alliance_id}/
        /// </summary>
        /// <param name="allianceId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Information>> Information(int alliance_id)
            => await Execute<Information>(_config, RequestSecurity.Public, RequestMethod.GET, $"/alliances/{alliance_id}/");

        /// <summary>
        /// /alliances/{alliance_id}/corporations/
        /// </summary>
        /// <param name="alliance_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Corporations(int alliance_id)
            => await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, $"/alliances/{alliance_id}/corporations/");

        /// <summary>
        /// /alliances/{alliance_id}/icons/
        /// </summary>
        /// <param name="alliance_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Images>> Icons(int alliance_id)
            => await Execute<Images>(_config, RequestSecurity.Public, RequestMethod.GET, $"/alliances/{alliance_id}/icons/");
    }
}