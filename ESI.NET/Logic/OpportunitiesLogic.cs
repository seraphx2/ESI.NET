using model = ESI.NET.Models.Opportunities;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;

namespace ESI.NET.Logic
{
    public class OpportunitiesLogic
    {
        private ESIConfig _config;
        private int character_id;

        public OpportunitiesLogic(ESIConfig config)
        {
            _config = config;

            if (_config.AuthorizedCharacter != null)
            {
                character_id = _config.AuthorizedCharacter.CharacterID;
            }
        }

        /// <summary>
        /// /opportunities/groups/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Groups()
        {
            var endpoint = "/opportunities/groups/";
            var response = await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /opportunities/groups/{group_id}/
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<model.Group>> Group(int group_id)
        {
            var endpoint = $"/opportunities/groups/{group_id}/";
            var response = await Execute<model.Group>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /opportunities/tasks/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Tasks()
        {
            var endpoint = "/opportunities/tasks/";
            var response = await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /opportunities/tasks/{task_id}/
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<model.Task>> Task(int task_id)
        {
            var endpoint = $"/opportunities/tasks/{task_id}/";
            var response = await Execute<model.Task>(_config, RequestSecurity.Public, RequestMethod.GET, endpoint);

            return response;
        }

        /// <summary>
        /// /characters/{character_id}/opportunities/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<model.CompletedTask>>> CompletedTasks()
        {
            var endpoint = $"/characters/{character_id}/opportunities/";
            var response = await Execute<List<model.CompletedTask>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, endpoint);

            return response;
        }
    }
}
