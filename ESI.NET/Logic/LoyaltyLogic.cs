using ESI.NET.Models.Loyalty;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Loyalty endpoints. Reach them through <see cref="EsiClient.Loyalty"/>.
    /// </summary>
    public class LoyaltyLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Loyalty endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public LoyaltyLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// List loyalty store offers. Return a list of offers from a specific corporation's loyalty
        /// store This route expires daily at 11:05.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /loyalty/stores/{corporation_id}/offers</c>.</para>
        /// </remarks>
        /// <param name="corporationId">The ID of the corporation.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Offer"/>.</returns>
        public async Task<EsiResponse<List<Offer>>> Offers(long corporationId, EsiCallOptions options = null)
            => await Execute<List<Offer>>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/loyalty/stores/{corporation_id}/offers/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", corporationId.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);


        /// <summary>
        /// Get loyalty points. Return a list of loyalty points for all corporations the character
        /// has worked for.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/loyalty/points</c>.</para>
        /// <para>Requires the <c>esi-characters.read_loyalty.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Points"/>.</returns>
        public async Task<EsiResponse<List<Points>>> Points(EsiCallOptions options)
            => await Execute<List<Points>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/loyalty/points/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}