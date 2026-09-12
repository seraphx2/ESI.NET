using ESI.NET.Models;
using ESI.NET.Models.Alliance;
using System.Collections.Generic;
using System.Globalization;
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
        public async Task<EsiResponse<long[]>> All(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/alliances/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /alliances/{alliance_id}/
        /// </summary>
        /// <param name="allianceId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Alliance>> Information(long allianceId, EsiCallOptions options = null)
            => await Execute<Alliance>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/alliances/{alliance_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "alliance_id", allianceId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /alliances/{alliance_id}/corporations/
        /// </summary>
        /// <param name="allianceId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Corporations(long allianceId, EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/alliances/{alliance_id}/corporations/",
                replacements: new Dictionary<string, string>()
                {
                    { "alliance_id", allianceId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /alliances/{alliance_id}/icons/
        /// </summary>
        /// <param name="allianceId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Images>> Icons(long allianceId, EsiCallOptions options = null)
            => await Execute<Images>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/alliances/{alliance_id}/icons/",
                replacements: new Dictionary<string, string>()
                {
                    { "alliance_id", allianceId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

    }
}