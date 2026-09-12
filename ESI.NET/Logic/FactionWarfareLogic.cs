using ESI.NET.Models.FactionWarfare;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class FactionWarfareLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public FactionWarfareLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /fw/wars/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<War>>> List(EsiCallOptions options = null)
            => await Execute<List<War>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/fw/wars/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /fw/stats/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Stat>>> Stats(EsiCallOptions options = null)
            => await Execute<List<Stat>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/fw/stats/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /fw/systems/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<FactionWarfareSystem>>> Systems(EsiCallOptions options = null)
            => await Execute<List<FactionWarfareSystem>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/fw/systems/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// fw/leaderboards/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Leaderboards<FactionTotal>>> Leaderboads(EsiCallOptions options = null)
            => await Execute<Leaderboards<FactionTotal>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/fw/leaderboards/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /fw/leaderboards/corporations/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Leaderboards<CorporationTotal>>> LeaderboardsForCorporations(EsiCallOptions options = null)
            => await Execute<Leaderboards<CorporationTotal>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/fw/leaderboards/corporations/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /fw/leaderboards/characters/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Leaderboards<CharacterTotal>>> LeaderboardsForCharacters(EsiCallOptions options = null)
            => await Execute<Leaderboards<CharacterTotal>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/fw/leaderboards/characters/",
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /corporations/{corporation_id}/fw/stats/
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<Stat>> StatsForCorporation(EsiCallOptions options)
            => await Execute<Stat>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/fw/stats/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/fw/stats/
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<Stat>> StatsForCharacter(EsiCallOptions options)
            => await Execute<Stat>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/fw/stats/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}