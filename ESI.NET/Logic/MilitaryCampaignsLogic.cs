using ESI.NET.Models.MilitaryCampaigns;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// Military Campaigns. Campaign and objective listings are public; the
    /// character's own objective progress needs <c>esi.activity.char:read</c>.
    /// </summary>
    public class MilitaryCampaignsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public MilitaryCampaignsLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>/military-campaigns/</summary>
        public async Task<EsiResponse<MilitaryCampaignList>> All(EsiCallOptions options = null)
            => await Execute<MilitaryCampaignList>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/military-campaigns/",
                options: options).ConfigureAwait(false);

        /// <summary>/military-campaigns/{campaign_id}/</summary>
        public async Task<EsiResponse<MilitaryCampaign>> Get(string campaign_id, EsiCallOptions options = null)
            => await Execute<MilitaryCampaign>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/military-campaigns/{campaign_id}/",
                replacements: new Dictionary<string, string>() { { "campaign_id", campaign_id } },
                options: options).ConfigureAwait(false);

        /// <summary>/military-campaigns/{campaign_id}/objectives/</summary>
        public async Task<EsiResponse<MilitaryObjectiveList>> Objectives(string campaign_id, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<MilitaryObjectiveList>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/military-campaigns/{campaign_id}/objectives/",
                replacements: new Dictionary<string, string>() { { "campaign_id", campaign_id } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString())),
                options: options).ConfigureAwait(false);

        /// <summary>/military-campaigns/{campaign_id}/objectives/{objective_id}/</summary>
        public async Task<EsiResponse<MilitaryObjective>> Objective(string campaign_id, string objective_id, EsiCallOptions options = null)
            => await Execute<MilitaryObjective>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/military-campaigns/{campaign_id}/objectives/{objective_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "campaign_id", campaign_id },
                    { "objective_id", objective_id }
                },
                options: options).ConfigureAwait(false);

        /// <summary>/characters/{character_id}/military-campaigns/objectives/ - scope esi.activity.char:read</summary>
        public async Task<EsiResponse<CharacterMilitaryObjectiveList>> CharacterObjectives(string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<CharacterMilitaryObjectiveList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/military-campaigns/objectives/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString() } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString())),
                options: options).ConfigureAwait(false);

        /// <summary>/characters/{character_id}/military-campaigns/objectives/{objective_id}/ - scope esi.activity.char:read</summary>
        public async Task<EsiResponse<CharacterMilitaryObjective>> CharacterObjective(string objective_id, EsiCallOptions options)
            => await Execute<CharacterMilitaryObjective>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/military-campaigns/objectives/{objective_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() },
                    { "objective_id", objective_id }
                },
                options: options).ConfigureAwait(false);

        private static string[] Cursor(params (string Key, string Value)[] pairs)
        {
            var list = new List<string>();
            foreach (var (key, value) in pairs)
                if (!string.IsNullOrEmpty(value))
                    list.Add($"{key}={value}");
            return list.Count > 0 ? list.ToArray() : null;
        }
    }
}
