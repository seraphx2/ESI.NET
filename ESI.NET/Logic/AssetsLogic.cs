using ESI.NET.Models.Assets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Assets endpoints. Reach them through <see cref="EsiClient.Assets"/>.
    /// </summary>
    public class AssetsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Assets endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public AssetsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>Return a list of the characters assets.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/assets</c>.</para>
        /// <para>Requires the <c>esi-assets.read_assets.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{character_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Item"/>.</returns>
        public async Task<EsiResponse<List<Item>>> ForCharacter(EsiCallOptions options)
            => await Execute<List<Item>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/assets/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get character asset locations. Return locations for a set of item ids, which you can get
        /// from character assets endpoint. Coordinates for items in hangars or stations are set to
        /// (0,0,0).
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>POST /characters/{character_id}/assets/locations</c>.</para>
        /// <para>Requires the <c>esi-assets.read_assets.v1</c> scope.</para>
        /// </remarks>
        /// <param name="itemIds">IDs of the asset items (at most 1000). Items of other owners, or
        /// that are not assembled, are rejected.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="ItemLocation"/>.</returns>
        public async Task<EsiResponse<List<ItemLocation>>> LocationsForCharacter(IReadOnlyList<long> itemIds, EsiCallOptions options)
            => await Execute<List<ItemLocation>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/characters/{character_id}/assets/locations/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                body: (itemIds ?? throw new ArgumentNullException(nameof(itemIds))).ToArray(),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get character asset names. Return names for a set of item ids, which you can get from
        /// character assets endpoint. Typically used for items that can customize names, like
        /// containers or ships.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>POST /characters/{character_id}/assets/names</c>.</para>
        /// <para>Requires the <c>esi-assets.read_assets.v1</c> scope.</para>
        /// </remarks>
        /// <param name="itemIds">IDs of the asset items (at most 1000). Items of other owners, or
        /// that are not assembled, are rejected.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="ItemName"/>.</returns>
        public async Task<EsiResponse<List<ItemName>>> NamesForCharacter(IReadOnlyList<long> itemIds, EsiCallOptions options)
            => await Execute<List<ItemName>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/characters/{character_id}/assets/names/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                body: (itemIds ?? throw new ArgumentNullException(nameof(itemIds))).ToArray(),
                options: options).ConfigureAwait(false);


        /// <summary>Return a list of the corporation assets.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/assets</c>.</para>
        /// <para>Requires the <c>esi-assets.read_corporation_assets.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Item"/>.</returns>
        public async Task<EsiResponse<List<Item>>> ForCorporation(EsiCallOptions options)
            => await Execute<List<Item>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/assets/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get corporation asset locations. Return locations for a set of item ids, which you can
        /// get from corporation assets endpoint. Coordinates for items in hangars or stations are
        /// set to (0,0,0).
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>POST /corporations/{corporation_id}/assets/locations</c>.</para>
        /// <para>Requires the <c>esi-assets.read_corporation_assets.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// </remarks>
        /// <param name="itemIds">IDs of the asset items (at most 1000). Items of other owners, or
        /// that are not assembled, are rejected.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="ItemLocation"/>.</returns>
        public async Task<EsiResponse<List<ItemLocation>>> LocationsForCorporation(IReadOnlyList<long> itemIds, EsiCallOptions options)
            => await Execute<List<ItemLocation>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/corporations/{corporation_id}/assets/locations/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                body: (itemIds ?? throw new ArgumentNullException(nameof(itemIds))).ToArray(),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get corporation asset names. Return names for a set of item ids, which you can get from
        /// corporation assets endpoint. Only valid for items that can customize names, like
        /// containers or ships.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>POST /corporations/{corporation_id}/assets/names</c>.</para>
        /// <para>Requires the <c>esi-assets.read_corporation_assets.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Director</c> corporation role.</para>
        /// </remarks>
        /// <param name="itemIds">IDs of the asset items (at most 1000). Items of other owners, or
        /// that are not assembled, are rejected.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="ItemName"/>.</returns>
        public async Task<EsiResponse<List<ItemName>>> NamesForCorporation(IReadOnlyList<long> itemIds, EsiCallOptions options)
            => await Execute<List<ItemName>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/corporations/{corporation_id}/assets/names/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                body: (itemIds ?? throw new ArgumentNullException(nameof(itemIds))).ToArray(),
                options: options).ConfigureAwait(false);
    }
}