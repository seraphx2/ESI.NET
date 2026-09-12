using ESI.NET.Models.Contacts;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class ContactsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public ContactsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Contact>>> ListForCharacter(EsiCallOptions options)
            => await Execute<List<Contact>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/contacts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Contact>>> ListForCorporation(EsiCallOptions options)
            => await Execute<List<Contact>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /alliances/{alliance_id}/contacts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Contact>>> ListForAlliance(EsiCallOptions options)
            => await Execute<List<Contact>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/alliances/{alliance_id}/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "alliance_id", options.Character.AllianceID.ToString() }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="contact_ids"></param>
        /// <param name="standing"></param>
        /// <param name="label_ids"></param>
        /// <param name="watched"></param>
        /// <returns></returns>
        public async Task<EsiResponse<long[]>> Add(long[] contact_ids, decimal standing, long[] label_ids = null, bool? watched = null, EsiCallOptions options = null)
        {
            var body = contact_ids;

            var parameters = new List<string>() { $"standing={standing}" };

            if (label_ids != null)
                parameters.Add($"label_ids={string.Join(",", label_ids)}");

            if (watched != null)
                parameters.Add($"watched={watched}");

            return await Execute<long[]>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/characters/{character_id}/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                parameters: parameters.ToArray(),
                body: body,
                options: options).ConfigureAwait(false);
        }

        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="contact_id"></param>
        /// <param name="standing"></param>
        /// <param name="label_id"></param>
        /// <param name="watched"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> Update(long[] contact_ids, decimal standing, long[] label_ids = null, bool? watched = null, EsiCallOptions options = null)
        {
            var body = contact_ids;

            var parameters = new List<string>() { $"standing={standing}" };

            if (label_ids != null)
                parameters.Add($"label_ids={string.Join(",", label_ids)}");

            if (watched != null)
                parameters.Add($"watched={watched}");

            return await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Put, "/characters/{character_id}/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                parameters: parameters.ToArray(),
                body: body,
                options: options).ConfigureAwait(false);
        }

        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="contact_ids"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> Delete(long[] contact_ids, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Delete, "/characters/{character_id}/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                parameters: new string[]
                {
                    $"contact_ids={string.Join(",", contact_ids)}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/contacts/labels/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Label>>> LabelsForCharacter(EsiCallOptions options)
            => await Execute<List<Label>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/contacts/labels/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/contacts/labels/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Label>>> LabelsForCorporation(EsiCallOptions options)
            => await Execute<List<Label>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/contacts/labels/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString() }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /alliances/{alliance_id}/contacts/labels/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Label>>> LabelsForAlliance(EsiCallOptions options)
            => await Execute<List<Label>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/alliances/{alliance_id}/contacts/labels/",
                replacements: new Dictionary<string, string>()
                {
                    { "alliance_id", options.Character.AllianceID.ToString() }
                },
                options: options).ConfigureAwait(false);
    }
}