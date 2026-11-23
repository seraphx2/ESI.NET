using ESI.NET.Models.Contacts;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// ESI's Contacts endpoints. Reach them through <see cref="EsiClient.Contacts"/>.
    /// </summary>
    public class ContactsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Contacts endpoints. <see cref="EsiClient"/> does this; there is no need to
        /// call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public ContactsLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>Return contacts of a character.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/contacts</c>.</para>
        /// <para>Requires the <c>esi-characters.read_contacts.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{character_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Contact"/>.</returns>
        public async Task<EsiResponse<List<Contact>>> ListForCharacter(EsiCallOptions options)
            => await Execute<List<Contact>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Return contacts of a corporation.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/contacts</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_contacts.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{corporation_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Contact"/>.</returns>
        public async Task<EsiResponse<List<Contact>>> ListForCorporation(EsiCallOptions options)
            => await Execute<List<Contact>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Return contacts of an alliance.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /alliances/{alliance_id}/contacts</c>.</para>
        /// <para>Requires the <c>esi-alliances.read_contacts.v1</c> scope.</para>
        /// <para>Paginated: <see cref="EsiCallOptions.Page"/> picks the page and
        /// <see cref="EsiResponse{T}.Pages"/> gives the page count.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill <c>{alliance_id}</c>.
        /// <see cref="EsiCallOptions.Page"/> picks the page.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Contact"/>.</returns>
        public async Task<EsiResponse<List<Contact>>> ListForAlliance(EsiCallOptions options)
            => await Execute<List<Contact>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/alliances/{alliance_id}/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "alliance_id", options.Character.AllianceID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Add contacts. Bulk add contacts with same settings.</summary>
        /// <remarks>
        /// <para>Calls <c>POST /characters/{character_id}/contacts</c>.</para>
        /// <para>Requires the <c>esi-characters.write_contacts.v1</c> scope.</para>
        /// </remarks>
        /// <param name="contactIds">IDs of the characters, corporations or alliances (at most
        /// 100).</param>
        /// <param name="standing">Standing for the contact.</param>
        /// <param name="labelIds">Add custom labels to the new contact.</param>
        /// <param name="watched">Whether the contact should be watched, note this is only effective
        /// on characters.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <c>long</c> values.</returns>
        public async Task<EsiResponse<long[]>> Add(long[] contactIds, decimal standing, long[] labelIds = null, bool? watched = null, EsiCallOptions options = null)
        {
            var body = contactIds;

            var parameters = new List<string>() { $"standing={standing}" };

            if (labelIds != null)
                parameters.Add($"label_ids={string.Join(",", labelIds)}");

            if (watched != null)
                parameters.Add($"watched={watched}");

            return await Execute<long[]>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Post, "/characters/{character_id}/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                parameters: parameters.ToArray(),
                body: body,
                options: options).ConfigureAwait(false);
        }

        /// <summary>Edit contacts. Bulk edit contacts with same settings.</summary>
        /// <remarks>
        /// <para>Calls <c>PUT /characters/{character_id}/contacts</c>.</para>
        /// <para>Requires the <c>esi-characters.write_contacts.v1</c> scope.</para>
        /// </remarks>
        /// <param name="contactIds">IDs of the characters, corporations or alliances (at most
        /// 100).</param>
        /// <param name="standing">Standing for the contact.</param>
        /// <param name="labelIds">Add custom labels to the contact.</param>
        /// <param name="watched">Whether the contact should be watched, note this is only effective
        /// on characters.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> Update(long[] contactIds, decimal standing, long[] labelIds = null, bool? watched = null, EsiCallOptions options = null)
        {
            var body = contactIds;

            var parameters = new List<string>() { $"standing={standing}" };

            if (labelIds != null)
                parameters.Add($"label_ids={string.Join(",", labelIds)}");

            if (watched != null)
                parameters.Add($"watched={watched}");

            return await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Put, "/characters/{character_id}/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                parameters: parameters.ToArray(),
                body: body,
                options: options).ConfigureAwait(false);
        }

        /// <summary>Bulk delete contacts.</summary>
        /// <remarks>
        /// <para>Calls <c>DELETE /characters/{character_id}/contacts</c>.</para>
        /// <para>Requires the <c>esi-characters.write_contacts.v1</c> scope.</para>
        /// </remarks>
        /// <param name="contactIds">A list of contacts to delete.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. Success is <c>204 No Content</c>, with no
        /// <see cref="EsiResponse{T}.Data"/>.</returns>
        public async Task<EsiResponse<string>> Delete(long[] contactIds, EsiCallOptions options)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Delete, "/characters/{character_id}/contacts/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                parameters: new string[]
                {
                    $"contact_ids={string.Join(",", contactIds)}"
                },
                options: options).ConfigureAwait(false);

        /// <summary>Return custom labels for a character's contacts.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/contacts/labels</c>.</para>
        /// <para>Requires the <c>esi-characters.read_contacts.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Label"/>.</returns>
        public async Task<EsiResponse<List<Label>>> LabelsForCharacter(EsiCallOptions options)
            => await Execute<List<Label>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/contacts/labels/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Return custom labels for a corporation's contacts.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/contacts/labels</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_contacts.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Label"/>.</returns>
        public async Task<EsiResponse<List<Label>>> LabelsForCorporation(EsiCallOptions options)
            => await Execute<List<Label>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/contacts/labels/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Return custom labels for an alliance's contacts.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /alliances/{alliance_id}/contacts/labels</c>.</para>
        /// <para>Requires the <c>esi-alliances.read_contacts.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{alliance_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds a list of
        /// <see cref="Label"/>.</returns>
        public async Task<EsiResponse<List<Label>>> LabelsForAlliance(EsiCallOptions options)
            => await Execute<List<Label>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/alliances/{alliance_id}/contacts/labels/",
                replacements: new Dictionary<string, string>()
                {
                    { "alliance_id", options.Character.AllianceID.ToString(CultureInfo.InvariantCulture) }
                },
                options: options).ConfigureAwait(false);
    }
}