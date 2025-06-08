﻿using ESI.NET.Models.Wars;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class WarsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public WarsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /wars/
        /// </summary>
        /// <param name="max_war_id">Only return wars with ID smaller than this</param>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> All(long max_war_id = 0, string eTag = null,
            CancellationToken cancellationToken = default)
        {
            var parameters = new List<string>();

            if (max_war_id > 0)
                parameters.Add($"max_war_id={max_war_id}");

            var response = await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/wars/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                parameters: parameters.ToArray());

            return response;
        }

        /// <summary>
        /// /wars/{warId}/
        /// </summary>
        /// <param name="war_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<War>> Information(int war_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<War>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/wars/{war_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "war_id", war_id.ToString() }
                });

        /// <summary>
        /// /wars/{warId}/killmails/
        /// </summary>
        /// <param name="war_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Models.Killmails.Killmail>>> Kills(int war_id, int page = 1,
            string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Models.Killmails.Killmail>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/wars/{war_id}/killmails/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "war_id", war_id.ToString() }
                },
                parameters: new string[]
                {
                    $"page={page}"
                });
    }
}