using ESI.NET.Models.Contacts;
using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class ContactsLogic
    {
        private HttpClient _client;
        private ESIConfig _config;
        AuthorizedCharacterData _data;

        private int character_id, corporation_id, alliance_id;

        public ContactsLogic(HttpClient client, ESIConfig config, AuthorizedCharacterData data = null)
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
        public async Task<ApiResponse<List<Contact>>> ListForCharacter(int page = 1)
            => await Execute<List<Contact>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contacts/", new string[]
            {
                $"page={page}"
            }, token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/contacts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Contact>>> ListForCorporation(int page = 1)
            => await Execute<List<Contact>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/contacts/", new string[]
            {
                $"page={page}"
            }, token: _data.Token);

        /// <summary>
        /// /alliances/{alliance_id}/contacts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Contact>>> ListForAlliance(int page = 1)
            => await Execute<List<Contact>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/alliances/{alliance_id}/contacts/", new string[]
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
        public async Task<ApiResponse<int[]>> Add(int contact_id, decimal standing, int? label_id = null, bool? watched = null)
        {
            var body = new int[] { contact_id };

            var parameters = new List<string>() { $"standing={standing}" };

            if (label_id != null)
                parameters.Add($"label_id={label_id}");

            if (watched != null)
                parameters.Add($"watched={watched}");

            var response = await Execute<int[]>(_client, _config, RequestSecurity.Authenticated, RequestMethod.POST, $"/characters/{character_id}/contacts/", body: body, parameters: parameters.ToArray(), token: _data.Token);

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["POST|/characters/{character_id}/contacts/"];

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="contact_id"></param>
        /// <param name="standing"></param>
        /// <param name="label_id"></param>
        /// <param name="watched"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> Update(int contact_id, decimal standing, int? label_id = null, bool? watched = null)
        {
            var body = new int[] { contact_id };

            var parameters = new List<string>() { $"standing={standing}" };

            if (label_id != null)
                parameters.Add($"label_id={label_id}");

            if (watched != null)
                parameters.Add($"watched={watched}");

            var response = await Execute<string>(_client, _config, RequestSecurity.Authenticated, RequestMethod.PUT, $"/characters/{character_id}/contacts/", body: body, parameters: parameters.ToArray(), token: _data.Token);

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["PUT|/characters/{character_id}/contacts/"];

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="contact_ids"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> Delete(int[] contact_ids)
        {
            var response = await Execute<string>(_client, _config, RequestSecurity.Authenticated, RequestMethod.DELETE, $"/characters/{character_id}/contacts/", new string[]
            {
                $"contact_ids={string.Join(",", contact_ids)}"
            }, token: _data.Token);

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["DELETE|/characters/{character_id}/contacts/"];

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/contacts/labels/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Label>>> LabelsForCharacter()
            => await Execute<List<Label>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contacts/labels/", token: _data.Token);

        /// <summary>
        /// /corporations/{corporation_id}/contacts/labels/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Label>>> LabelsForCorporation()
            => await Execute<List<Label>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/contacts/labels/", token: _data.Token);

        /// <summary>
        /// /alliances/{alliance_id}/contacts/labels/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Label>>> LabelsForAlliance()
            => await Execute<List<Label>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/alliances/{alliance_id}/contacts/labels/", token: _data.Token);
    }
}