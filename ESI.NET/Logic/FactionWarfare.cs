using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.FactionWarfare;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class FactionWarfareLogic : IFactionWarfareLogic
    {
        private ESIConfig _config;
        private int corporation_id, character_id;

        public FactionWarfareLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                corporation_id = _config.AuthorizedCharacter.CorporationID;
                character_id = _config.AuthorizedCharacter.CharacterID;
            }
        }

        /// <summary>
        /// /fw/wars/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<War>>> List()
            => await Execute<List<War>>(_config, RequestSecurity.Public, RequestMethod.GET, "/fw/wars/");

        /// <summary>
        /// /fw/stats/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Stat>>> Stats()
            => await Execute<List<Stat>>(_config, RequestSecurity.Public, RequestMethod.GET, "/fw/stats/");

        /// <summary>
        /// /fw/systems/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<FactionWarfareSystem>>> Systems()
            => await Execute<List<FactionWarfareSystem>>(_config, RequestSecurity.Public, RequestMethod.GET, "/fw/systems/");

        /// <summary>
        /// fw/leaderboards/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<Leaderboards<FactionTotal>>> Leaderboads()
            => await Execute<Leaderboards<FactionTotal>>(_config, RequestSecurity.Public, RequestMethod.GET, "/fw/leaderboards/");

        /// <summary>
        /// /fw/leaderboards/corporations/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<Leaderboards<CorporationTotal>>> LeaderboardsForCorporations()
            => await Execute<Leaderboards<CorporationTotal>>(_config, RequestSecurity.Public, RequestMethod.GET, "/fw/leaderboards/corporations/");

        /// <summary>
        /// /fw/leaderboards/characters/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<Leaderboards<CharacterTotal>>> LeaderboardsForCharacters()
            => await Execute<Leaderboards<CharacterTotal>>(_config, RequestSecurity.Public, RequestMethod.GET, "/fw/leaderboards/characters/");

        /// <summary>
        /// /corporations/{corporation_id}/fw/stats/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<Stat>> StatsForCorporation()
            => await Execute<Stat>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/fw/stats/");

        /// <summary>
        /// /characters/{character_id}/fw/stats/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<Stat>> StatsForCharacter()
            => await Execute<Stat>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/fw/stats/");
    }
}