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

        public CosmeticsLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>/paragon-hub/skinr/ - the public Paragon Hub SKINR listings.</summary>
        public async Task<EsiResponse<ParagonListingPage>> ParagonListings(string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ParagonListingPage>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/paragon-hub/skinr/",
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>/paragon-hub/skinr/alliances/{alliance_id}/</summary>
        public async Task<EsiResponse<ParagonListingPage>> ParagonAllianceListings(long allianceId, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ParagonListingPage>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/paragon-hub/skinr/alliances/{alliance_id}/",
                replacements: new Dictionary<string, string>() { { "alliance_id", allianceId.ToString(CultureInfo.InvariantCulture) } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>/paragon-hub/skinr/characters/{character_id}/</summary>
        public async Task<EsiResponse<ParagonListingPage>> ParagonCharacterListings(long characterId, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ParagonListingPage>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/paragon-hub/skinr/characters/{character_id}/",
                replacements: new Dictionary<string, string>() { { "character_id", characterId.ToString(CultureInfo.InvariantCulture) } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>/paragon-hub/skinr/corporations/{corporation_id}/</summary>
        public async Task<EsiResponse<ParagonListingPage>> ParagonCorporationListings(long corporationId, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ParagonListingPage>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/paragon-hub/skinr/corporations/{corporation_id}/",
                replacements: new Dictionary<string, string>() { { "corporation_id", corporationId.ToString(CultureInfo.InvariantCulture) } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>/characters/{character_id}/paragon-hub/skinr/ - the caller's own listings (each carries a target).</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<ParagonListingPage>> MyParagonListings(string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ParagonListingPage>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/paragon-hub/skinr/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>/cosmetics/skinr/{skinr_id}/ - a public SKINR design lookup.</summary>
        public async Task<EsiResponse<SkinrDesign>> Skinr(string skinrId, EsiCallOptions options = null)
            => await Execute<SkinrDesign>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/cosmetics/skinr/{skinr_id}/",
                replacements: new Dictionary<string, string>() { { "skinr_id", skinrId } },
                options: options).ConfigureAwait(false);

        /// <summary>/characters/{character_id}/cosmetics/skinr/ - the character's SKINR licenses.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<SkinrLicenses>> MyLicenses(EsiCallOptions options)
            => await Execute<SkinrLicenses>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/cosmetics/skinr/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) } },
                options: options).ConfigureAwait(false);

        /// <summary>/characters/{character_id}/cosmetics/skinr/components/ - the character's SKINR components.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
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
