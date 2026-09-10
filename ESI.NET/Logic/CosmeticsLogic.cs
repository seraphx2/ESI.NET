using ESI.NET.Models.Cosmetics;
using System.Collections.Generic;
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
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString())),
                options: options);

        /// <summary>/paragon-hub/skinr/alliances/{alliance_id}/</summary>
        public async Task<EsiResponse<ParagonListingPage>> ParagonAllianceListings(long alliance_id, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ParagonListingPage>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/paragon-hub/skinr/alliances/{alliance_id}/",
                replacements: new Dictionary<string, string>() { { "alliance_id", alliance_id.ToString() } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString())),
                options: options);

        /// <summary>/paragon-hub/skinr/characters/{character_id}/</summary>
        public async Task<EsiResponse<ParagonListingPage>> ParagonCharacterListings(long character_id, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ParagonListingPage>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/paragon-hub/skinr/characters/{character_id}/",
                replacements: new Dictionary<string, string>() { { "character_id", character_id.ToString() } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString())),
                options: options);

        /// <summary>/paragon-hub/skinr/corporations/{corporation_id}/</summary>
        public async Task<EsiResponse<ParagonListingPage>> ParagonCorporationListings(long corporation_id, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ParagonListingPage>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/paragon-hub/skinr/corporations/{corporation_id}/",
                replacements: new Dictionary<string, string>() { { "corporation_id", corporation_id.ToString() } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString())),
                options: options);

        /// <summary>/characters/{character_id}/paragon-hub/skinr/ - the caller's own listings (each carries a target).</summary>
        public async Task<EsiResponse<ParagonListingPage>> MyParagonListings(string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<ParagonListingPage>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/paragon-hub/skinr/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString() } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString())),
                options: options);

        /// <summary>/cosmetics/skinr/{skinr_id}/ - a public SKINR design lookup.</summary>
        public async Task<EsiResponse<SkinrDesign>> Skinr(string skinr_id, EsiCallOptions options = null)
            => await Execute<SkinrDesign>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/cosmetics/skinr/{skinr_id}/",
                replacements: new Dictionary<string, string>() { { "skinr_id", skinr_id } },
                options: options);

        /// <summary>/characters/{character_id}/cosmetics/skinr/ - the character's SKINR licenses.</summary>
        public async Task<EsiResponse<SkinrLicenses>> MyLicenses(EsiCallOptions options)
            => await Execute<SkinrLicenses>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/cosmetics/skinr/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString() } },
                options: options);

        /// <summary>/characters/{character_id}/cosmetics/skinr/components/ - the character's SKINR components.</summary>
        public async Task<EsiResponse<SkinrComponents>> MyComponents(EsiCallOptions options)
            => await Execute<SkinrComponents>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/cosmetics/skinr/components/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString() } },
                options: options);

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
