using ESI.NET.Models.MilitaryCampaigns;
using System.Collections.Generic;
using System.Globalization;
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

        /// <summary>
        /// Creates the Military Campaigns endpoints. <see cref="EsiClient"/> does this; there is no
        /// need to call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public MilitaryCampaignsLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>List military campaigns. Listing of all active military campaigns.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /military-campaigns</c>.</para>
        /// </remarks>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="MilitaryCampaignList"/>.</returns>
        public async Task<EsiResponse<MilitaryCampaignList>> All(EsiCallOptions options = null)
            => await Execute<MilitaryCampaignList>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/military-campaigns/",
                options: options).ConfigureAwait(false);

        /// <summary>Get the details of a military campaign.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /military-campaigns/{campaign_id}</c>.</para>
        /// </remarks>
        /// <param name="campaignId">The ID of the military campaign.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="MilitaryCampaign"/>.</returns>
        public async Task<EsiResponse<MilitaryCampaign>> Get(string campaignId, EsiCallOptions options = null)
            => await Execute<MilitaryCampaign>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/military-campaigns/{campaign_id}/",
                replacements: new Dictionary<string, string>() { { "campaign_id", campaignId } },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List military campaign objectives. Listing of all active, completed or expired
        /// objectives of a military campaign.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /military-campaigns/{campaign_id}/objectives</c>.</para>
        /// <para>Cursor-paginated: page with <c>before</c> / <c>after</c> and <c>limit</c>.</para>
        /// </remarks>
        /// <param name="campaignId">The ID of the military campaign.</param>
        /// <param name="after">Return records from after this cursor (mutual exclusive with
        /// 'before'). '0' to start from the beginning.</param>
        /// <param name="before">Return records from before this cursor (mutual exclusive with
        /// 'after'). '0' to start from the end.</param>
        /// <param name="limit">The amount of records to retrieve per request.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="MilitaryObjectiveList"/>.</returns>
        public async Task<EsiResponse<MilitaryObjectiveList>> Objectives(string campaignId, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<MilitaryObjectiveList>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/military-campaigns/{campaign_id}/objectives/",
                replacements: new Dictionary<string, string>() { { "campaign_id", campaignId } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>Get the details of an objective of a military campaign.</summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /military-campaigns/{campaign_id}/objectives/{objective_id}</c>.</para>
        /// </remarks>
        /// <param name="campaignId">The ID of the military campaign.</param>
        /// <param name="objectiveId">The ID of the objective.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="MilitaryObjective"/>.</returns>
        public async Task<EsiResponse<MilitaryObjective>> Objective(string campaignId, string objectiveId, EsiCallOptions options = null)
            => await Execute<MilitaryObjective>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/military-campaigns/{campaign_id}/objectives/{objective_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "campaign_id", campaignId },
                    { "objective_id", objectiveId }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List character participation in military campaigns. Listing of the military campaign
        /// objectives the character has participated in.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/military-campaigns/objectives</c>.</para>
        /// <para>Requires the <c>esi.activity.char:read</c> scope.</para>
        /// <para>Cursor-paginated: page with <c>before</c> / <c>after</c> and <c>limit</c>.</para>
        /// </remarks>
        /// <param name="after">Return records from after this cursor (mutual exclusive with
        /// 'before'). '0' to start from the beginning.</param>
        /// <param name="before">Return records from before this cursor (mutual exclusive with
        /// 'after'). '0' to start from the end.</param>
        /// <param name="limit">The amount of records to retrieve per request.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="CharacterMilitaryObjectiveList"/>.</returns>
        public async Task<EsiResponse<CharacterMilitaryObjectiveList>> CharacterObjectives(string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<CharacterMilitaryObjectiveList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/military-campaigns/objectives/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>Show your participation in a military campaign objective.</summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /characters/{character_id}/military-campaigns/objectives/{objective_id}</c>.</para>
        /// <para>Requires the <c>esi.activity.char:read</c> scope.</para>
        /// </remarks>
        /// <param name="objectiveId">The ID of the objective.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="CharacterMilitaryObjective"/>.</returns>
        public async Task<EsiResponse<CharacterMilitaryObjective>> CharacterObjective(string objectiveId, EsiCallOptions options)
            => await Execute<CharacterMilitaryObjective>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/military-campaigns/objectives/{objective_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "objective_id", objectiveId }
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
