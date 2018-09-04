using ESI.NET.Models.Wars;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class WarsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        public WarsLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>
        /// /wars/
        /// </summary>
        /// <param name="max_war_id">Only return wars with ID smaller than this</param>
        /// <returns></returns>
        public async Task<EsiResponse<List<int>>> All(long max_war_id = 0)
        {
            var parameters = new List<string>();

            if (max_war_id > 0)
                parameters.Add($"max_war_id={max_war_id}");

            var response = await Execute<List<int>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/wars/", parameters: parameters.ToArray());

            return response;
        }

        /// <summary>
        /// /wars/{warId}/
        /// </summary>
        /// <param name="war_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Information>> Information(int war_id)
            => await Execute<Information>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/wars/{war_id}/", replacements: new Dictionary<string, string>()
            {
                { "war_id", war_id.ToString() }
            });

        /// <summary>
        /// /wars/{warId}/killmails/
        /// </summary>
        /// <param name="war_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Models.Killmails.Killmail>>> Kills(int war_id, int page = 1)
            => await Execute<List<Models.Killmails.Killmail>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/wars/{war_id}/killmails/", replacements: new Dictionary<string, string>()
            {
                { "war_id", war_id.ToString() }
            }, parameters: new string[]
            {
                $"page={page}"
            });
    }
}