﻿using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;
using opportunities = ESI.NET.Models.Opportunities;

namespace ESI.NET.Logic
{
    public class OpportunitiesLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly AuthorizedCharacterData _data;
        private readonly int character_id;

        public OpportunitiesLogic(HttpClient client, EsiConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
                character_id = data.CharacterID;
        }

        /// <summary>
        /// /opportunities/groups/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Groups(string eTag = null, CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/opportunities/groups/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /opportunities/groups/{group_id}/
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<opportunities.Group>> Group(int group_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<opportunities.Group>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/opportunities/groups/{group_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "group_id", group_id.ToString() }
                });

        /// <summary>
        /// /opportunities/tasks/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Tasks(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/opportunities/tasks/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /opportunities/tasks/{task_id}/
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<opportunities.Task>> Task(int task_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<opportunities.Task>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/opportunities/tasks/{task_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "task_id", task_id.ToString() }
                });

        /// <summary>
        /// /characters/{character_id}/opportunities/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<opportunities.CompletedTask>>> CompletedTasks(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<opportunities.CompletedTask>>(_client, _config, RequestSecurity.Authenticated,
                HttpMethod.Get, "/characters/{character_id}/opportunities/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                token: _data.Token);
    }
}