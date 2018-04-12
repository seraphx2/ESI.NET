using ESI.NET.Logic.Interfaces;
using ESI.NET.Models.Contacts;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class ContactsLogic : IContactsLogic
    {
        private ESIConfig _config;
        private int character_id, corporation_id, alliance_id;

        public ContactsLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                character_id = _config.AuthorizedCharacter.CharacterID;
                corporation_id = _config.AuthorizedCharacter.CorporationID;
                alliance_id = _config.AuthorizedCharacter.CorporationID;
            }
        }

        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Contact>>> List(int page = 1)
            => await Execute<List<Contact>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contacts/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /corporations/{corporation_id}/contacts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Contact>>> ListForCorporation(int page = 1)
            => await Execute<List<Contact>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/corporations/{corporation_id}/contacts/", new string[]
            {
                $"page={page}"
            });

        /// <summary>
        /// /alliances/{alliance_id}/contacts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Contact>>> ListForAlliance(int page = 1)
            => await Execute<List<Contact>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/alliances/{alliance_id}/contacts/", new string[]
            {
                $"page={page}"
            });

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

            var response = await Execute<int[]>(_config, RequestSecurity.Authenticated, RequestMethod.POST, $"/characters/{character_id}/contacts/", body: body, parameters: parameters.ToArray());

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

            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.PUT, $"/characters/{character_id}/contacts/", body: body, parameters: parameters.ToArray());

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
            var response = await Execute<string>(_config, RequestSecurity.Authenticated, RequestMethod.DELETE, $"/characters/{character_id}/contacts/", new string[]
            {
                $"contact_ids={string.Join(",", contact_ids)}"
            });

            if (response.StatusCode == HttpStatusCode.NoContent)
                response.Message = Dictionaries.NoContentMessages["DELETE|/characters/{character_id}/contacts/"];

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/contacts/labels/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Label>>> Labels()
            => await Execute<List<Label>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/contacts/labels/");
    }
}