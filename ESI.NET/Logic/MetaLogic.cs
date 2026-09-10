using ESI.NET.Models.Meta;
using System.Net.Http;
using System.Threading.Tasks;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Logic
{
    /// <summary>ESI's own metadata - all public.</summary>
    public class MetaLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;

        public MetaLogic(HttpClient client, EsiConfig config) { _client = client; _config = config; }

        /// <summary>/meta/changelog/ - every route change, keyed by compatibility date.</summary>
        public async Task<EsiResponse<MetaChangelog>> Changelog(EsiCallOptions options = null)
            => await Execute<MetaChangelog>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/meta/changelog/",
                options: options);

        /// <summary>/meta/compatibility-dates/ - the published snapshot dates.</summary>
        public async Task<EsiResponse<MetaCompatibilityDates>> CompatibilityDates(EsiCallOptions options = null)
            => await Execute<MetaCompatibilityDates>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/meta/compatibility-dates/",
                options: options);

        /// <summary>/meta/name/ - the current ESI product name and its history.</summary>
        public async Task<EsiResponse<MetaName>> Name(EsiCallOptions options = null)
            => await Execute<MetaName>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/meta/name/",
                options: options);

        /// <summary>/meta/status/ - per-route health.</summary>
        public async Task<EsiResponse<MetaStatus>> Status(EsiCallOptions options = null)
            => await Execute<MetaStatus>(_client, _config, RequestSecurity.Public, HttpMethod.Get, "/meta/status/",
                options: options);
    }
}
