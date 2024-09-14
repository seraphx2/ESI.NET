using ESI.NET.Models.FactionWarfare;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Interfaces.Logic;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class FactionWarfareLogic : IFactionWarfareLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly AuthorizedCharacterData _data;
        private readonly int character_id, corporation_id;

        public FactionWarfareLogic(HttpClient client, EsiConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
            {
                corporation_id = data.CorporationID;
                character_id = data.CharacterID;
            }
        }

        /// <summary>
        /// /fw/wars/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<War>>> List(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<War>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/fw/wars/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /fw/stats/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Stat>>> Stats(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<Stat>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/fw/stats/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /fw/systems/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<FactionWarfareSystem>>> Systems(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<List<FactionWarfareSystem>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/fw/systems/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// fw/leaderboards/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Leaderboards<FactionTotal>>> Leaderboads(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Leaderboards<FactionTotal>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/fw/leaderboards/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /fw/leaderboards/corporations/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Leaderboards<CorporationTotal>>> LeaderboardsForCorporations(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Leaderboards<CorporationTotal>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/fw/leaderboards/corporations/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /fw/leaderboards/characters/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Leaderboards<CharacterTotal>>> LeaderboardsForCharacters(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Leaderboards<CharacterTotal>>(_client, _config, RequestSecurity.Public, HttpMethod.Get,
                "/fw/leaderboards/characters/",
                eTag: eTag,
                cancellationToken: cancellationToken);

        /// <summary>
        /// /corporations/{corporation_id}/fw/stats/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Stat>> StatsForCorporation(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Stat>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/corporations/{corporation_id}/fw/stats/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"corporation_id", corporation_id.ToString()}
                },
                token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/fw/stats/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<Stat>> StatsForCharacter(string eTag = null,
            CancellationToken cancellationToken = default)
            => await Execute<Stat>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get,
                "/characters/{character_id}/fw/stats/",
                eTag: eTag,
                cancellationToken: cancellationToken,
                replacements: new Dictionary<string, string>()
                {
                    {"character_id", character_id.ToString()}
                },
                token: _data.Token);
    }
}