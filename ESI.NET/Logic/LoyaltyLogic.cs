using ESI.NET.Models.Loyalty;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class LoyaltyLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public LoyaltyLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /loyalty/stores/{corporation_id}/offers/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Offer>>> Offers(long corporationId, EsiCallOptions options = null)
            => await Execute<List<Offer>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/loyalty/stores/{corporation_id}/offers/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporationId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /characters/{character_id}/loyalty/points/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Points>>> Points(EsiCallOptions options)
            => await Execute<List<Points>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/loyalty/points/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}