﻿using ESI.NET.Models;
using ESI.NET.Models.Corporation;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class CorporationLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly AuthorizedCharacterData _data;
        private readonly int corporation_id;

        public CorporationLogic(HttpClient client, EsiConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
                corporation_id = data.CorporationID;
        }

        /// <summary>
        /// /corporations/npccorps/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> NpcCorps(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/corporations/npccorps/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /corporations/{corporation_id}/
        /// </summary>
        /// <param name="corporation_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Corporation>> Information(int corporation_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Corporation>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/corporations/{corporation_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                });

        /// <summary>
        /// /corporations/{corporation_id}/alliancehistory/
        /// </summary>
        /// <param name="corporation_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<AllianceHistory>>> AllianceHistory(int corporation_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<AllianceHistory>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/corporations/{corporation_id}/alliancehistory/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                });

        /// <summary>
        /// /corporations/{corporation_id}/blueprints/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Blueprint>>> Blueprints(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Blueprint>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/blueprints/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                parameters: new string[]
                {
                    $"page={page}"
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/containers/logs/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<ContainerLog>>> ContainerLogs(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<ContainerLog>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/containers/logs/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                parameters: new string[]
                {
                    $"page={page}"
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/divisions/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Divisions>> Divisions(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Divisions>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/divisions/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/facilities/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Facility>>> Facilities(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Facility>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/facilities/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/icons/
        /// </summary>
        /// <param name="corporationId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Images>> Icons(int corporation_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Images>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/corporations/{corporation_id}/icons/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                });

        /// <summary>
        /// /corporations/{corporation_id}/medals/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Medal>>> Medals(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Medal>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/medals/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                }, parameters: new string[]
                {
                    $"page={page}"
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/medals/issued/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<IssuedMedal>>> MedalsIssued(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<IssuedMedal>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/medals/issued/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                parameters: new string[]
                {
                    $"page={page}"
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/members/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Members(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int[]>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/members/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/members/limit/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int>> MemberLimit(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<int>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/members/limit/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/members/titles/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<MemberTitles>>> MemberTitles(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<MemberTitles>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/members/titles/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/membertracking/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<MemberInfo>>> MemberTracking(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<MemberInfo>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/membertracking/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/roles/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<CharacterRoles>>> Roles(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<CharacterRoles>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/roles/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/roles/history/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<CharacterRolesHistory>>> RolesHistory(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<CharacterRolesHistory>>(_client, _config, RequestSecurity.Authenticated,
                HttpMethod.Get, "/corporations/{corporation_id}/roles/history/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/shareholders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Shareholder>>> Shareholders(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Shareholder>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/shareholders/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                }, parameters: new string[]
                {
                    $"page={page}"
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/standings/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Standing>> Standings(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Standing>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/standings/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                parameters: new string[]
                {
                    $"page={page}"
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/starbases/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Starbase>>> Starbases(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Starbase>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/starbases/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                parameters: new string[]
                {
                    $"page={page}"
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/starbases/{starbase_id}/
        /// </summary>
        /// <param name="starbase_id"></param>
        /// <param name="system_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<StarbaseInfo>> Starbase(long starbase_id, int system_id, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<StarbaseInfo>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/starbases/{starbase_id}/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() },
                    { "starbase_id", starbase_id.ToString() }
                },
                parameters: new string[]
                {
                    $"system_id={system_id}"
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/structures/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Structure>>> Structures(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Structure>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/structures/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                parameters: new string[]
                {
                    $"page={page}"
                },
                token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/titles/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Title>>> Titles(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Title>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/titles/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                token: _data.Token);
    }
}