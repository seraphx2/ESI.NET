using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;
using Opportunities = ESI.NET.Models.Opportunities;

namespace ESI.NET.Logic
{
    public class OpportunitiesLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        
        public OpportunitiesLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// /opportunities/groups/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Groups(EsiCallOptions options = null)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/opportunities/groups/",
                options: options);


        /// <summary>
        /// /opportunities/groups/{group_id}/
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Opportunities.Group>> Group(int group_id, EsiCallOptions options = null)
            => await Execute<Opportunities.Group>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/opportunities/groups/{group_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "group_id", group_id.ToString() }
                },
                options: options);


        /// <summary>
        /// /opportunities/tasks/
        /// </summary>
        /// <returns></returns>
        public async Task<EsiResponse<int[]>> Tasks(EsiCallOptions options = null)
            => await Execute<int[]>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/opportunities/tasks/",
                options: options);


        /// <summary>
        /// /opportunities/tasks/{task_id}/
        /// </summary>
        /// <param name="task_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<Opportunities.Task>> Task(int task_id, EsiCallOptions options = null)
            => await Execute<Opportunities.Task>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/opportunities/tasks/{task_id}/",
                replacements: new Dictionary<string, string>()
                {
                    { "task_id", task_id.ToString() }
                },
                options: options);


        /// <summary>
        /// /characters/{character_id}/opportunities/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        public async Task<EsiResponse<List<Opportunities.CompletedTask>>> CompletedTasks(EsiCallOptions options)
            => await Execute<List<Opportunities.CompletedTask>>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/opportunities/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString() }
                },
                options: options);
    }
}