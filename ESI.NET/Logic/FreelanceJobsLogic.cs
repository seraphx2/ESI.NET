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

        public FreelanceJobsLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>
        /// /freelance-jobs/ - the public listing, optionally scoped to one corporation.
        /// </summary>
        public async Task<EsiResponse<FreelanceJobList>> All(long? corporationId = null, string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<FreelanceJobList>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/freelance-jobs/",
                parameters: Cursor(("corporation_id", corporationId?.ToString(CultureInfo.InvariantCulture)), ("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /freelance-jobs/{job_id}/ - full public detail for one job.
        /// </summary>
        public async Task<EsiResponse<FreelanceJob>> Get(string jobId, EsiCallOptions options = null)
            => await Execute<FreelanceJob>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/freelance-jobs/{job_id}/",
                replacements: new Dictionary<string, string>() { { "job_id", jobId } },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/freelance-jobs/ - jobs the character is participating in.
        /// </summary>
        public async Task<EsiResponse<FreelanceJobList>> ForCharacter(EsiCallOptions options)
            => await Execute<FreelanceJobList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/freelance-jobs/",
                replacements: new Dictionary<string, string>() { { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) } },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /characters/{character_id}/freelance-jobs/{job_id}/participation/ - the character's participation record.
        /// </summary>
        public async Task<EsiResponse<FreelanceParticipation>> CharacterParticipation(string jobId, EsiCallOptions options)
            => await Execute<FreelanceParticipation>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/characters/{character_id}/freelance-jobs/{job_id}/participation/",
                replacements: new Dictionary<string, string>()
                {
                    { "character_id", options.Character.CharacterID.ToString(CultureInfo.InvariantCulture) },
                    { "job_id", jobId }
                },
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/freelance-jobs/ - jobs the corporation has posted.
        /// </summary>
        public async Task<EsiResponse<FreelanceJobList>> ForCorporation(string after = null, string before = null, int? limit = null, EsiCallOptions options = null)
            => await Execute<FreelanceJobList>(_client, _config, RequestSecurity.Authenticated, HttpMethod.Get, "/corporations/{corporation_id}/freelance-jobs/",
                replacements: new Dictionary<string, string>() { { "corporation_id", options.Character.CorporationID.ToString(CultureInfo.InvariantCulture) } },
                parameters: Cursor(("after", after), ("before", before), ("limit", limit?.ToString(CultureInfo.InvariantCulture))),
                options: options).ConfigureAwait(false);

        /// <summary>
        /// /corporations/{corporation_id}/freelance-jobs/{job_id}/participants/ - a page of participants.
        /// </summary>
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
