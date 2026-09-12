using ESI.NET.Models.Assets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class AssetsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public AssetsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /characters/{character_id}/assets/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<List<Item>>> ForCharacter(EsiCallOptions options)
            => await Execute<List<Item>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/assets/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/assets/locations/
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<List<ItemLocation>>> LocationsForCharacter(List<long> itemIds, EsiCallOptions options)
            => await Execute<List<ItemLocation>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/characters/{character_id}/assets/locations/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                body: (itemIds ?? throw new ArgumentNullException(nameof(itemIds))).ToArray(),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/assets/names/
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<List<ItemName>>> NamesForCharacter(List<long> itemIds, EsiCallOptions options)
            => await Execute<List<ItemName>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/characters/{character_id}/assets/names/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                body: (itemIds ?? throw new ArgumentNullException(nameof(itemIds))).ToArray(),
                options: options).ConfigureAwait(false);


        /// <summary>
        /// /corporations/{corporation_id}/assets/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<List<Item>>> ForCorporation(EsiCallOptions options)
            => await Execute<List<Item>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/assets/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/assets/locations/
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<List<ItemLocation>>> LocationsForCorporation(List<long> itemIds, EsiCallOptions options)
            => await Execute<List<ItemLocation>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/corporations/{corporation_id}/assets/locations/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                body: (itemIds ?? throw new ArgumentNullException(nameof(itemIds))).ToArray(),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/assets/names/
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods",
            Justification = "options is deliberately optional (= null) on every endpoint method - the vast majority of calls need no special options at all. EsiRequest.Execute<T> already substitutes a default when it's null; throwing here would turn the library's single most common call shape into a guaranteed crash.")]
        public async Task<EsiResponse<List<ItemName>>> NamesForCorporation(List<long> itemIds, EsiCallOptions options)
            => await Execute<List<ItemName>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/corporations/{corporation_id}/assets/names/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                body: (itemIds ?? throw new ArgumentNullException(nameof(itemIds))).ToArray(),
                options: options).ConfigureAwait(false);
    }
}