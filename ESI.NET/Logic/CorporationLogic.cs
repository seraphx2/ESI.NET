using ESI.NET.Models;
using ESI.NET.Models.Corporation;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class CorporationLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public CorporationLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /corporations/npccorps/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> NpcCorps(EsiCallOptions options = null)
            => await Execute<long[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/corporations/npccorps/",
                options: options);


        /// <summary>
        /// /corporations/{corporation_id}/
        /// </summary>
        /// <param name="corporation_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Corporation>> Information(int corporation_id, EsiCallOptions options = null)
            => await Execute<Corporation>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/corporations/{corporation_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                options: options);


        /// <summary>
        /// /corporations/{corporation_id}/alliancehistory/
        /// </summary>
        /// <param name="corporation_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<AllianceHistory>>> AllianceHistory(int corporation_id, EsiCallOptions options = null)
            => await Execute<List<AllianceHistory>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/corporations/{corporation_id}/alliancehistory/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                options: options);


        /// <summary>
        /// /corporations/{corporation_id}/blueprints/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Blueprint>>> Blueprints(EsiCallOptions options)
            => await Execute<List<Blueprint>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/blueprints/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/containers/logs/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<ContainerLog>>> ContainerLogs(EsiCallOptions options)
            => await Execute<List<ContainerLog>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/containers/logs/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/divisions/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Divisions>> Divisions(EsiCallOptions options)
            => await Execute<Divisions>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/divisions/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/facilities/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Facility>>> Facilities(EsiCallOptions options)
            => await Execute<List<Facility>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/facilities/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/icons/
        /// </summary>
        /// <param name="corporationId"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Images>> Icons(int corporation_id, EsiCallOptions options = null)
            => await Execute<Images>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/corporations/{corporation_id}/icons/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporation_id.ToString() }
                },
                options: options);


        /// <summary>
        /// /corporations/{corporation_id}/medals/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Medal>>> Medals(EsiCallOptions options)
            => await Execute<List<Medal>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/medals/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/medals/issued/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<IssuedMedal>>> MedalsIssued(EsiCallOptions options)
            => await Execute<List<IssuedMedal>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/medals/issued/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/members/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Members(EsiCallOptions options)
            => await Execute<long[]>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/members/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/members/limit/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<long>> MemberLimit(EsiCallOptions options)
            => await Execute<long>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/members/limit/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/members/titles/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<MemberTitles>>> MemberTitles(EsiCallOptions options)
            => await Execute<List<MemberTitles>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/members/titles/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/membertracking/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<MemberInfo>>> MemberTracking(EsiCallOptions options)
            => await Execute<List<MemberInfo>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/membertracking/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/roles/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<CharacterRoles>>> Roles(EsiCallOptions options)
            => await Execute<List<CharacterRoles>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/roles/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/roles/history/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<CharacterRolesHistory>>> RolesHistory(EsiCallOptions options)
            => await Execute<List<CharacterRolesHistory>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/roles/history/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/shareholders/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Shareholder>>> Shareholders(EsiCallOptions options)
            => await Execute<List<Shareholder>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/shareholders/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/standings/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Standing>>> Standings(EsiCallOptions options)
            => await Execute<List<Standing>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/standings/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/starbases/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Starbase>>> Starbases(EsiCallOptions options)
            => await Execute<List<Starbase>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/starbases/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/starbases/{starbase_id}/
        /// </summary>
        /// <param name="starbase_id"></param>
        /// <param name="system_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<StarbaseInfo>> Starbase(long starbase_id, int system_id, EsiCallOptions options)
            => await Execute<StarbaseInfo>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/starbases/{starbase_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() },
                    { "starbase_id", starbase_id.ToString() }
                },
                parameters: new string[]
                {
                    $"system_id={system_id}"
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/structures/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Structure>>> Structures(EsiCallOptions options)
            => await Execute<List<Structure>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/structures/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);

        /// <summary>
        /// /corporations/{corporation_id}/titles/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Title>>> Titles(EsiCallOptions options)
            => await Execute<List<Title>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/titles/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options);
    }
}