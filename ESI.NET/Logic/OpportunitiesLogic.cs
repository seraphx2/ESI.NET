using ESI.NET.Logic.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ESI.NET.ApiRequest;
using model = ESI.NET.Models.Opportunities;

namespace ESI.NET.Logic
{
    public class OpportunitiesLogic : IOpportunitiesLogic
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
            => await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, "/opportunities/groups/");

        /// <summary>
        /// /opportunities/groups/{group_id}/
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<model.Group>> Group(int group_id)
            => await Execute<model.Group>(_config, RequestSecurity.Public, RequestMethod.GET, $"/opportunities/groups/{group_id}/");

        /// <summary>
        /// /opportunities/tasks/
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<int>>> Tasks()
            => await Execute<List<int>>(_config, RequestSecurity.Public, RequestMethod.GET, "/opportunities/tasks/");

        /// <summary>
        /// /opportunities/tasks/{task_id}/
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<model.Task>> Task(int task_id)
            => await Execute<model.Task>(_config, RequestSecurity.Public, RequestMethod.GET, $"/opportunities/tasks/{task_id}/");

        /// <summary>
        /// /characters/{character_id}/opportunities/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<model.CompletedTask>>> CompletedTasks()
            => await Execute<List<model.CompletedTask>>(_config, RequestSecurity.Authenticated, RequestMethod.GET, $"/characters/{character_id}/opportunities/");
    }
}