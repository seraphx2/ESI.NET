using ESI.NET.Models.Skills;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Skills endpoints. Reach them through <see cref="EsiClient.Skills"/>.
    /// </summary>
    public class SkillsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Skills endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public SkillsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>Return attributes of a character.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/attributes</c>.</para>
        /// <para>Requires the <c>esi-skills.read_skills.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="Attributes"/>.</returns>
        public async Task<EsiResponse<Attributes>> Attributes(EsiCallOptions options)
            => await Execute<Attributes>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/attributes/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get character skills. List all trained skills for the given character. Skills returned
        /// by this route can be out-of-date if the character hasn't logged in since one or more
        /// skills completed training. Use the /skillqueue route to check for skills that completed
        /// training. Entries that are in the past need to be applied on top of this list to get an
        /// accurate view of the character's current skills.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/skills</c>.</para>
        /// <para>Requires the <c>esi-skills.read_skills.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="SkillDetails"/>.</returns>
        public async Task<EsiResponse<SkillDetails>> List(EsiCallOptions options)
            => await Execute<SkillDetails>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/skills/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get character's skill queue. List the configured skill queue for the given character.
        /// Entries that have their finish time in the past are completed, but aren't updated in the
        /// "/skills" route yet. This will happen the next time the character logs in.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/skillqueue</c>.</para>
        /// <para>Requires the <c>esi-skills.read_skillqueue.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="SkillQueueItem"/>.</returns>
        public async Task<EsiResponse<List<SkillQueueItem>>> Queue(EsiCallOptions options)
            => await Execute<List<SkillQueueItem>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/skillqueue/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}