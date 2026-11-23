using ESI.NET.Models.Cosmetics;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// Ship customization (SKINR) and the Paragon Hub marketplace. The global
    /// listing and a design lookup are public; everything else needs
    /// <c>esi.cosmetic.char:read</c>.
    /// </summary>
    public class CosmeticsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Paragon Hub endpoints. <see cref="EsiClient"/> does this; there is no need
        /// to call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public CosmeticsLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>
        /// List public Paragon Hub SKINR listings. Browse the SKINR listings publicly available on
        /// the Paragon Hub.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /paragon-hub/skinr</c>.</para>
        /// <para>Cursor-paginated: page with <c>before</c> / <c>after</c> and <c>limit</c>.</para>
        /// </remarks>
        /// <param name="after">Return records from after this cursor (mutual exclusive with
        /// 'before'). '0' to start from the beginning.</param>
        /// <param name="before">Return records from before this cursor (mutual exclusive with
        /// 'after'). '0' to start from the end.</param>
        /// <param name="limit">The amount of records to retrieve per request.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="ParagonListingPage"/>.</returns>
        public async Task<EsiResponse<ParagonListingPage>> ParagonListings(string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ParagonListingPage>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/paragon-hub/skinr/",
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List Paragon Hub SKINR listings targeted at an alliance. Browse the SKINR listings on
        /// the Paragon Hub that are visible to the given alliance.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /paragon-hub/skinr/alliances/{alliance_id}</c>.</para>
        /// <para>Requires the <c>esi.cosmetic.char:read</c> scope.</para>
        /// <para>Cursor-paginated: page with <c>before</c> / <c>after</c> and <c>limit</c>.</para>
        /// </remarks>
        /// <param name="allianceId">The ID of the alliance the listings are targeted at.</param>
        /// <param name="after">Return records from after this cursor (mutual exclusive with
        /// 'before'). '0' to start from the beginning.</param>
        /// <param name="before">Return records from before this cursor (mutual exclusive with
        /// 'after'). '0' to start from the end.</param>
        /// <param name="limit">The amount of records to retrieve per request.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="ParagonListingPage"/>.</returns>
        public async Task<EsiResponse<ParagonListingPage>> ParagonAllianceListings(long allianceId, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ParagonListingPage>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/paragon-hub/skinr/alliances/{alliance_id}/",
                replacements: new Dictionary<string, string>() { { "alliance_id", allianceId.ToString(CultureInfo.InvariantCulture) } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List Paragon Hub SKINR listings targeted at a character. Browse the SKINR listings on
        /// the Paragon Hub that are visible to the given character.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /paragon-hub/skinr/characters/{character_id}</c>.</para>
        /// <para>Requires the <c>esi.cosmetic.char:read</c> scope.</para>
        /// <para>Cursor-paginated: page with <c>before</c> / <c>after</c> and <c>limit</c>.</para>
        /// </remarks>
        /// <param name="characterId">The ID of the character the listings are targeted at.</param>
        /// <param name="after">Return records from after this cursor (mutual exclusive with
        /// 'before'). '0' to start from the beginning.</param>
        /// <param name="before">Return records from before this cursor (mutual exclusive with
        /// 'after'). '0' to start from the end.</param>
        /// <param name="limit">The amount of records to retrieve per request.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="ParagonListingPage"/>.</returns>
        public async Task<EsiResponse<ParagonListingPage>> ParagonCharacterListings(long characterId, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ParagonListingPage>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/paragon-hub/skinr/characters/{character_id}/",
                replacements: new Dictionary<string, string>() { { "character_id", characterId.ToString(CultureInfo.InvariantCulture) } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List Paragon Hub SKINR listings targeted at a corporation. Browse the SKINR listings on
        /// the Paragon Hub that are visible to the given corporation.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /paragon-hub/skinr/corporations/{corporation_id}</c>.</para>
        /// <para>Requires the <c>esi.cosmetic.char:read</c> scope.</para>
        /// <para>Cursor-paginated: page with <c>before</c> / <c>after</c> and <c>limit</c>.</para>
        /// </remarks>
        /// <param name="corporationId">The ID of the corporation the listings are targeted
        /// at.</param>
        /// <param name="after">Return records from after this cursor (mutual exclusive with
        /// 'before'). '0' to start from the beginning.</param>
        /// <param name="before">Return records from before this cursor (mutual exclusive with
        /// 'after'). '0' to start from the end.</param>
        /// <param name="limit">The amount of records to retrieve per request.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="ParagonListingPage"/>.</returns>
        public async Task<EsiResponse<ParagonListingPage>> ParagonCorporationListings(long corporationId, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ParagonListingPage>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/paragon-hub/skinr/corporations/{corporation_id}/",
                replacements: new Dictionary<string, string>() { { "corporation_id", corporationId.ToString(CultureInfo.InvariantCulture) } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List a character's Paragon Hub SKINR listings. List the SKINR listings a character has
        /// posted on the Paragon Hub.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/paragon-hub/skinr</c>.</para>
        /// <para>Requires the <c>esi.cosmetic.char:read</c> scope.</para>
        /// <para>Cursor-paginated: page with <c>before</c> / <c>after</c> and <c>limit</c>.</para>
        /// </remarks>
        /// <param name="after">Return records from after this cursor (mutual exclusive with
        /// 'before'). '0' to start from the beginning.</param>
        /// <param name="before">Return records from before this cursor (mutual exclusive with
        /// 'after'). '0' to start from the end.</param>
        /// <param name="limit">The amount of records to retrieve per request.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="ParagonListingPage"/>.</returns>
        public async Task<EsiResponse<ParagonListingPage>> MyParagonListings(string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ParagonListingPage>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/paragon-hub/skinr/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>All attributes of a SKINR license.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /cosmetics/skinr/{skinr_id}</c>.</para>
        /// </remarks>
        /// <param name="skinrId">SKINR identifier.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="SkinrDesign"/>.</returns>
        public async Task<EsiResponse<SkinrDesign>> Skinr(string skinrId, EsiCallOptions options = null)
            => await Execute<SkinrDesign>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/cosmetics/skinr/{skinr_id}/",
                replacements: new Dictionary<string, string>() { { "skinr_id", skinrId } },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List a character's owned SKINR licenses. Listing of all SKINR licenses you own.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/cosmetics/skinr</c>.</para>
        /// <para>Requires the <c>esi.cosmetic.char:read</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="SkinrLicenses"/>.</returns>
        public async Task<EsiResponse<SkinrLicenses>> MyLicenses(EsiCallOptions options)
            => await Execute<SkinrLicenses>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/cosmetics/skinr/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) } },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List a character's owned SKINR component licenses. Listing of all SKINR component
        /// licenses you own.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/cosmetics/skinr/components</c>.</para>
        /// <para>Requires the <c>esi.cosmetic.char:read</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="SkinrComponents"/>.</returns>
        public async Task<EsiResponse<SkinrComponents>> MyComponents(EsiCallOptions options)
            => await Execute<SkinrComponents>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/cosmetics/skinr/components/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) } },
                options: options).ConfigureAwait(false);

        private static string[] Cursor(params (string Key, string Value)[] pairs)
        {
            var list = new List<string>();
            foreach (var (key, value) in pairs)
                if (!string.IsNullOrEmpty(value))
                    list.Add($"{key}={value}");
            return list.Count > 0 ? list.ToArray() : null;
        }
    }
}
