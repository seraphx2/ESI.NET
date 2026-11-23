using ESI.NET.Models.FreelanceJobs;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>
    /// Freelance Jobs. The public listing needs no scope; the character and
    /// corporation views need <c>esi-characters.read_freelance_jobs.v1</c> and
    /// <c>esi-corporations.read_freelance_jobs.v1</c> respectively.
    /// </summary>
    public class FreelanceJobsLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        /// <summary>
        /// Creates the Freelance Jobs endpoints. <see cref="EsiClient"/> does this; there is no
        /// need to call it.
        /// </summary>
        /// <param name="client">The <see cref="System.Net.Http.HttpClient"/> requests are sent
        /// with.</param>
        /// <param name="config">The ESI.NET configuration.</param>
        public FreelanceJobsLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>List freelance jobs. Listing of all public freelance jobs.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /freelance-jobs</c>.</para>
        /// <para>Cursor-paginated: page with <c>before</c> / <c>after</c> and <c>limit</c>.</para>
        /// </remarks>
        /// <param name="corporationId">Filter on corporation ID.</param>
        /// <param name="after">Return records from after this cursor (mutual exclusive with
        /// 'before'). '0' to start from the beginning.</param>
        /// <param name="before">Return records from before this cursor (mutual exclusive with
        /// 'after'). '0' to start from the end.</param>
        /// <param name="limit">The amount of records to retrieve per request.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="FreelanceJobList"/>.</returns>
        public async Task<EsiResponse<FreelanceJobList>> All(long? corporationId = null, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<FreelanceJobList>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/freelance-jobs/",
                parameters: Cursor(("corporation_id", corporationId?.ToString(CultureInfo.InvariantCulture)), ("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// Get freelance job details. Get the details of a freelance job. Jobs without an ACL
        /// (public jobs) does not require authentication. Jobs with an ACL requires authentication,
        /// and requires that the character is: - An active participant of the job, or - A freelance
        /// job manager for the corporation that owns the job.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /freelance-jobs/{job_id}</c>.</para>
        /// </remarks>
        /// <param name="jobId">The ID of the freelance job.</param>
        /// <param name="options">Per-call options: cancellation,
        /// <see cref="EsiCallOptions.IfNoneMatch"/>,
        /// <see cref="EsiCallOptions.BypassCache"/>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="FreelanceJob"/>.</returns>
        public async Task<EsiResponse<FreelanceJob>> Get(string jobId, EsiCallOptions options = null)
            => await Execute<FreelanceJob>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/freelance-jobs/{job_id}/",
                replacements: new Dictionary<string, string>() { { "job_id", jobId } },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// List character freelance jobs. Listing of all freelance jobs you are actively
        /// participating in.
        /// </summary>
        /// <remarks>
        /// <para>Calls <c>GET /characters/{character_id}/freelance-jobs</c>.</para>
        /// <para>Requires the <c>esi-characters.read_freelance_jobs.v1</c> scope.</para>
        /// </remarks>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="FreelanceJobList"/>.</returns>
        public async Task<EsiResponse<FreelanceJobList>> ForCharacter(EsiCallOptions options)
            => await Execute<FreelanceJobList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/freelance-jobs/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) } },
                options: options).ConfigureAwait(false);

        /// <summary>Show your participation in a freelance job.</summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /characters/{character_id}/freelance-jobs/{job_id}/participation</c>.</para>
        /// <para>Requires the <c>esi-characters.read_freelance_jobs.v1</c> scope.</para>
        /// </remarks>
        /// <param name="jobId">The ID of the freelance job.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{character_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="FreelanceParticipation"/>.</returns>
        public async Task<EsiResponse<FreelanceParticipation>> CharacterParticipation(string jobId, EsiCallOptions options)
            => await Execute<FreelanceParticipation>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/freelance-jobs/{job_id}/participation/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "job_id", jobId }
                },
                options: options).ConfigureAwait(false);

        /// <summary>Listing of all freelance jobs for your corporation.</summary>
        /// <remarks>
        /// <para>Calls <c>GET /corporations/{corporation_id}/freelance-jobs</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_freelance_jobs.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Project_Manager</c> corporation role.</para>
        /// <para>Cursor-paginated: page with <c>before</c> / <c>after</c> and <c>limit</c>.</para>
        /// </remarks>
        /// <param name="after">Return records from after this cursor (mutual exclusive with
        /// 'before'). '0' to start from the beginning.</param>
        /// <param name="before">Return records from before this cursor (mutual exclusive with
        /// 'after'). '0' to start from the end.</param>
        /// <param name="limit">The amount of records to retrieve per request.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="FreelanceJobList"/>.</returns>
        public async Task<EsiResponse<FreelanceJobList>> ForCorporation(string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<FreelanceJobList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/freelance-jobs/",
                replacements: new Dictionary<string, string>() { { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>Listing of all participants of a freelance job.</summary>
        /// <remarks>
        /// <para>Calls <c>GET
        /// /corporations/{corporation_id}/freelance-jobs/{job_id}/participants</c>.</para>
        /// <para>Requires the <c>esi-corporations.read_freelance_jobs.v1</c> scope.</para>
        /// <para>The character must also hold the <c>Project_Manager</c> corporation role.</para>
        /// <para>Cursor-paginated: page with <c>before</c> / <c>after</c> and <c>limit</c>.</para>
        /// </remarks>
        /// <param name="jobId">The ID of the job.</param>
        /// <param name="after">Return records from after this cursor (mutual exclusive with
        /// 'before'). '0' to start from the beginning.</param>
        /// <param name="before">Return records from before this cursor (mutual exclusive with
        /// 'after'). '0' to start from the end.</param>
        /// <param name="limit">The amount of records to retrieve per request.</param>
        /// <param name="options">Per-call options. <see cref="EsiCallOptions.Character"/> is
        /// required: its access token authorizes the call and its IDs fill
        /// <c>{corporation_id}</c>.</param>
        /// <returns>The response. On success, <see cref="EsiResponse{T}.Data"/> holds the
        /// <see cref="FreelanceParticipants"/>.</returns>
        public async Task<EsiResponse<FreelanceParticipants>> CorporationParticipants(string jobId, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<FreelanceParticipants>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/freelance-jobs/{job_id}/participants/",
                replacements: new Dictionary<string, string>()
                {
                    { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) },
                    { "job_id", jobId }
                },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        private static string[] Cursor(params (string Key, string Value)[] pairs)
        {
            var list = new List<string>();
            foreach (var (key, value) in pairs)
                if (!string.IsNullOrEmpty(value))
                    list.Add($"{key}={value}");
            return list.Count > 0 ? list.ToArray() : null;
        }
    }
}
