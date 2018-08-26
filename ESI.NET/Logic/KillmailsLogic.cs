using ESI.NET.Models.Killmails;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class KillmailsLogic : BaseLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly AuthorizedCharacterData _data;
        private readonly int character_id, corporation_id;

        public KillmailsLogic(HttpClient client, EsiConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
            {
                character_id = data.CharacterID;
                corporation_id = data.CorporationID;
            }
        }

        /// <summary>
        /// /characters/{character_id}/killmails/recent/
        /// </summary>
        /// <param name="max_kill_id">Only return killmails with ID smaller than this</param>
        /// <param name="max_count">How many killmails to return at maximum</param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Killmail>>> ForCharacter(int? max_kill_id = 0, int max_count = 50)
        {
            var parameters = new List<string>() { $"max_count={max_count}" };

            if (max_kill_id > 0)
                parameters.Add($"max_kill_id={max_kill_id}");

            var response = await Execute<List<Killmail>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/killmails/recent/", parameters: parameters.ToArray(), token: _data.Token);

            return response;
        }

        /// <summary>
        /// /corporations/{corporation_id}/killmails/recent/
        /// </summary>
        /// <param name="max_kill_id">Only return killmails with ID smaller than this</param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Killmail>>> ForCorporation(int? max_kill_id = 0)
        {
            var parameters = new List<string>();

            if (max_kill_id > 0)
                parameters.Add($"max_kill_id={max_kill_id}");

            var response = await Execute<List<Killmail>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/killmails/recent/", parameters: parameters.ToArray(), token: _data.Token);

            return response;
        }

        /// <summary>
        /// /killmails/{killmail_id}/{killmail_hash}/
        /// </summary>
        /// <param name="killmail_hash">The killmail hash for verification</param>
        /// <param name="killmail_id">The killmail ID to be queried</param>
        /// <returns></returns>
        public async Task<EsiResponse<Information>> Information(string killmail_hash, int killmail_id)
            => await Execute<Information>(_client, _config, RequestSecurity.Public, RequestMethod.GET, $"/killmails/{killmail_id}/{killmail_hash}/");
    }
}