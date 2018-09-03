using ESI.NET.Models.Contacts;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    public class ContactsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly AuthorizedCharacterData _data;

        private readonly int character_id, corporation_id, alliance_id;

        public ContactsLogic(HttpClient client, EsiConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (_data != null)
            {
                character_id = _data.CharacterID;
                corporation_id = _data.CorporationID;
                alliance_id = _data.CorporationID;
            }
        }

        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Contact>>> ListForCharacter(int page = 1)
            => await Execute<List<Contact>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contacts/", parameters: new string[]
            {
                $"page={page}"
            }, token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/contacts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Contact>>> ListForCorporation(int page = 1)
            => await Execute<List<Contact>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/contacts/", parameters: new string[]
            {
                $"page={page}"
            }, token: _data.Token);

        /// <summary>
        /// /alliances/{alliance_id}/contacts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Contact>>> ListForAlliance(int page = 1)
            => await Execute<List<Contact>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/alliances/{alliance_id}/contacts/", parameters: new string[]
            {
                $"page={page}"
            }, token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="contact_id"></param>
        /// <param name="standing"></param>
        /// <param name="label_id"></param>
        /// <param name="watched"></param>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Add(int contact_id, decimal standing, int? label_id = null, bool? watched = null)
        {
            var body = new int[] { contact_id };

            var parameters = new List<string>() { $"standing={standing}" };

            if (label_id != null)
                parameters.Add($"label_id={label_id}");

            if (watched != null)
                parameters.Add($"watched={watched}");

            return await Execute<int[]>(_client, _config, RequestSecurity.Authenticated, RequestMethod.POST, $"/characters/{character_id}/contacts/", noContent: NoContentMessages["POST|/characters/{character_id}/contacts/"], body: body, parameters: parameters.ToArray(), token: _data.Token);
        }

        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="contact_id"></param>
        /// <param name="standing"></param>
        /// <param name="label_id"></param>
        /// <param name="watched"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> Update(int contact_id, decimal standing, int? label_id = null, bool? watched = null)
        {
            var body = new int[] { contact_id };

            var parameters = new List<string>() { $"standing={standing}" };

            if (label_id != null)
                parameters.Add($"label_id={label_id}");

            if (watched != null)
                parameters.Add($"watched={watched}");

            return await Execute<string>(_client, _config, RequestSecurity.Authenticated, RequestMethod.PUT, $"/characters/{character_id}/contacts/", noContent: NoContentMessages["PUT|/characters/{character_id}/contacts/"], body: body, parameters: parameters.ToArray(), token: _data.Token);
        }

        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="contact_ids"></param>
        /// <returns></returns>
        public async Task<EsiResponse<string>> Delete(int[] contact_ids)
            => await Execute<string>(_client, _config, RequestSecurity.Authenticated, RequestMethod.DELETE, $"/characters/{character_id}/contacts/", noContent: NoContentMessages["DELETE|/characters/{character_id}/contacts/"], parameters: new string[]
            {
                $"contact_ids={string.Join(",", contact_ids)}"
            }, token: _data.Token);

        /// <summary>
        /// /characters/{character_id}/contacts/labels/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Label>>> LabelsForCharacter()
            => await Execute<List<Label>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contacts/labels/", token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/contacts/labels/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Label>>> LabelsForCorporation()
            => await Execute<List<Label>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/contacts/labels/", token: _data.Token);

        /// <summary>
        /// /alliances/{alliance_id}/contacts/labels/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<List<Label>>> LabelsForAlliance()
            => await Execute<List<Label>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/alliances/{alliance_id}/contacts/labels/", token: _data.Token);
    }
}