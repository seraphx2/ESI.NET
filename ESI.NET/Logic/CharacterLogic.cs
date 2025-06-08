﻿using ESI.NET.Models;
using ESI.NET.Models.Character;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class CharacterLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly AuthorizedCharacterData _data;
        private readonly int character_id;

        public CharacterLogic(HttpClient client, EsiConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
                character_id = data.CharacterID;
        }

        /// <summary>
        /// /characters/affiliation/
        /// </summary>
        /// <param name="characterIds">dynamic = long</param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Affiliation>>> Affiliation(int[] character_ids,
            CancellationToken cancellationToken = default)
            => await Execute<List<Affiliation>>(_client, _config, RequestSecurity.Public, HttpMethod.Post,
                "/characters/affiliation/",
                cancellationToken: cancellationToken,
                body: character_ids);

        /// <summary>
        /// /characters/names/
        /// </summary>
        /// <param name="characterIds"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Character>>> Names(int[] character_ids, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Character>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/characters/names/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                parameters: new string[]
                {
                    $"character_ids={string.Join(",", character_ids)}"
                });

        /// <summary>
        /// /characters/{character_id}/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Information>> Information(int character_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Information>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/characters/{character_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                });

        /// <summary>
        /// /characters/{character_id}/agents_research/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Agent>>> AgentsResearch(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Agent>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/agents_research/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/blueprints/
        /// </summary>
        /// <param name="page">Which page of results to return</param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Blueprint>>> Blueprints(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Blueprint>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/blueprints/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                token: _data.Token,
                parameters: new string[]
                {
                    $"page={page}"
                });

        /// <summary>
        /// /characters/{character_id}/chat_channels/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<ChatChannel>>> ChatChannels(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<ChatChannel>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/chat_channels/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/corporationhistory/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<CorporationHistory>>> CorporationHistory(int character_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<CorporationHistory>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/characters/{character_id}/corporationhistory/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                });

        /// <summary>
        /// /characters/{character_id}/cspa/
        /// </summary>
        /// <param name="character_ids">The target characters to calculate the charge for</param>
        /// <returns></returns>
        public async Task<EsiResponse<decimal>> CSPA(object character_ids, CancellationToken cancellationToken = default)
            => await Execute<decimal>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post,
                "/characters/{character_id}/cspa/",
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                body: character_ids,
                token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/fatigue/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Fatigue>> Fatigue(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Fatigue>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/fatigue/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/medals/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Medal>>> Medals(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Medal>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/medals/",
                eTag: eTag,
                cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/notifications/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Notification>>> Notifications(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Notification>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/notifications/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/notifications/contacts/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<ContactNotification>>> ContactNotifications()
            => await Execute<List<ContactNotification>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/notifications/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/portrait/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Images>> Portrait(int character_id)
            => await Execute<Images>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/characters/{character_id}/portrait/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                });

        /// <summary>
        /// /characters/{character_id}/roles/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Roles>> Roles()
            => await Execute<Roles>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/roles/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/standings/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Standing>>> Standings()
            => await Execute<List<Standing>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/standings/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/titles/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Title>>> Titles()
            => await Execute<List<Title>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/titles/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", character_id.ToString() }
                },
                token: _data.Token);
    }
}