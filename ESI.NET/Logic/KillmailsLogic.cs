using ESI.NET.Models.Killmails;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class KillmailsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public KillmailsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /characters/{character_id}/killmails/recent/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Killmail>>> ForCharacter(EsiCallOptions options)
            => await Execute<List<Killmail>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/killmails/recent/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/killmails/recent/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Killmail>>> ForCorporation(EsiCallOptions options)
            => await Execute<List<Killmail>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/killmails/recent/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /killmails/{killmail_id}/{killmail_hash}/
        /// </summary>
        /// <param name="killmail_hash">The killmail hash for verification</param>
        /// <param name="killmail_id">The killmail ID to be queried</param>
        /// <returns></returns>
        public async Task<EsiResponse<Information>> Information(string killmail_hash, long killmail_id, EsiCallOptions options = null)
            => await Execute<Information>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/killmails/{killmail_id}/{killmail_hash}/",
                replacements: new Dictionary<string, string>()
                {
                    { "killmail_id", killmail_id.ToString(CultureInfo.InvariantCulture) },
                    { "killmail_hash", killmail_hash.ToString() }
                },
                options: options).ConfigureAwait(false);

    }
}