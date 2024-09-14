using ESI.NET.Models;
using ESI.NET.Models.Alliance;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Interfaces.Logic;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class AllianceLogic : IAllianceLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public AllianceLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /alliances/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> All(string eTag = null, CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/alliances/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /alliances/{alliance_id}/
        /// </summary>
        /// <param name="allianceId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Alliance>> Information(int alliance_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Alliance>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/alliances/{alliance_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"alliance_id", alliance_id.ToString()}
                });

        /// <summary>
        /// /alliances/{alliance_id}/corporations/
        /// </summary>
        /// <param name="alliance_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Corporations(int alliance_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/alliances/{alliance_id}/corporations/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"alliance_id", alliance_id.ToString()}
                });

        /// <summary>
        /// /alliances/{alliance_id}/icons/
        /// </summary>
        /// <param name="alliance_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Images>> Icons(int alliance_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Images>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/alliances/{alliance_id}/icons/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"alliance_id", alliance_id.ToString()}
                });
    }
}