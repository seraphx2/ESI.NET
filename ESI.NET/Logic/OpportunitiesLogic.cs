using ESI.NET.Models.SSO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;
using model = ESI.NET.Models.Opportunities;

namespace ESI.NET.Logic
{
    public class OpportunitiesLogic
    {
        private HttpClient _client;
        private ESIConfig _config;
        private AuthorizedCharacterData _data;
        private int character_id;
        
        public OpportunitiesLogic(HttpClient client, ESIConfig config, AuthorizedCharacterData data = null)
        {
            _client = client;
            _config = config;
            _data = data;

            if (data != null)
                character_id = data.CharacterID;
        }

        /// <summary>
        /// /opportunities/groups/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Groups()
            => await Execute<List<int>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/opportunities/groups/");

        /// <summary>
        /// /opportunities/groups/{group_id}/
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<model.Group>> Group(int group_id)
            => await Execute<model.Group>(_client, _config, RequestSecurity.Public, RequestMethod.GET, $"/opportunities/groups/{group_id}/");

        /// <summary>
        /// /opportunities/tasks/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Tasks()
            => await Execute<List<int>>(_client, _config, RequestSecurity.Public, RequestMethod.GET, "/opportunities/tasks/");

        /// <summary>
        /// /opportunities/tasks/{task_id}/
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<model.Task>> Task(int task_id)
            => await Execute<model.Task>(_client, _config, RequestSecurity.Public, RequestMethod.GET, $"/opportunities/tasks/{task_id}/");

        /// <summary>
        /// /characters/{character_id}/opportunities/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<model.CompletedTask>>> CompletedTasks()
            => await Execute<List<model.CompletedTask>>(_client, _config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/opportunities/", token: _data.Token);
    }
}