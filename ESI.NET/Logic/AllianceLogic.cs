using ESI.NET.Models;
using ESI.NET.Models.Alliance;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class AllianceLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public AllianceLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>
        /// /alliances/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<int>>> All()
            => await Execute<List<int>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/alliances/");

        /// <summary>
        /// /alliances/names/
        /// </summary>
        /// <param name="alliance_ids"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Alliance>>> Names(int[] alliance_ids)
            => await Execute<List<Alliance>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/alliances/names/", parameters: new string[]
            {
                $"alliance_ids={string.Join(",", alliance_ids)}"
            });

        /// <summary>
        /// /alliances/{alliance_id}/
        /// </summary>
        /// <param name="allianceId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Information>> Information(int alliance_id)
            => await Execute<Information>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/alliances/{alliance_id}/", replacements: new Dictionary<string, string>()
            {
                { "alliance_id", alliance_id.ToString() }
            });

        /// <summary>
        /// /alliances/{alliance_id}/corporations/
        /// </summary>
        /// <param name="alliance_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<int>>> Corporations(int alliance_id)
            => await Execute<List<int>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/alliances/{alliance_id}/corporations/", replacements: new Dictionary<string, string>()
            {
                { "alliance_id", alliance_id.ToString() }
            });

        /// <summary>
        /// /alliances/{alliance_id}/icons/
        /// </summary>
        /// <param name="alliance_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Images>> Icons(int alliance_id)
            => await Execute<Images>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/alliances/{alliance_id}/icons/", replacements: new Dictionary<string, string>()
            {
                { "alliance_id", alliance_id.ToString() }
            });
    }
}