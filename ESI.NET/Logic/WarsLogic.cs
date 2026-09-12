using ESI.NET.Models.Wars;
using System.Collections.Generic;
using System.Globalization;
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
        /// <param name="maxWarId">Only return wars with ID smaller than this</param>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> All(long maxWarId = 0, EsiCallOptions options = null)
        {
            var parameters = new List<string>();

            if (maxWarId > 0)
                parameters.Add($"max_war_id={maxWarId}");

            var response = await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/wars/",
                parameters: parameters.ToArray(),
                options: options).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// /wars/{warId}/
        /// </summary>
        /// <param name="warId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<War>> Information(long warId, EsiCallOptions options = null)
            => await Execute<War>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/wars/{war_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "war_id", warId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /wars/{warId}/killmails/
        /// </summary>
        /// <param name="warId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Models.Killmails.Killmail>>> Kills(long warId, EsiCallOptions options = null)
            => await Execute<List<Models.Killmails.Killmail>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/wars/{war_id}/killmails/",
                replacements: new Dictionary<string, string>()
                {
                    { "war_id", warId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}