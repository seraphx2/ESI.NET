using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Loyalty;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class LoyaltyLogic : ILoyaltyLogic
    {
        private ESIConfig _config;
        private int character_id, corporation_id, alliance_id;

        public LoyaltyLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
                character_id = _config.AuthorizedCharacter.CharacterID;
        }

        /// <summary>
        /// /loyalty/stores/{corporation_id}/offers/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Offer>>> Offers(int corporation_id)
            => await Execute<List<Offer>>(_config, RequestSecurity.Public, RequestMethod.GET, $"/loyalty/stores/{corporation_id}/offers/");

        /// <summary>
        /// /characters/{character_id}/loyalty/points/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Points>>> Points()
            => await Execute<List<Points>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/loyalty/points/");
    }
}