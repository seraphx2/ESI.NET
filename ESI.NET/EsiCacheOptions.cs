using System;

namespace ESI.NET
{
    /// <summary>
    /// Settings for the response cache turned on by <c>AddEsiCaching()</c>.
    /// </summary>
    public sealed class EsiCacheOptions
    {
        /// <summary>
        /// Prepended to every cache key, so ESI.NET's entries can share a store (e.g. a Redis
        /// database) with other data. Default <c>esi.net:</c>.
        /// </summary>
        public string KeyPrefix { get; set; } = "esi.net:";

        /// <summary>
        /// How long an entry is kept after ESI's <c>Expires</c> has passed. A stale entry is not
        /// served as-is; its <c>ETag</c> is sent as <c>If-None-Match</c>, and a <c>304</c> brings it
        /// back to life without re-downloading the body. Default one hour.
        /// </summary>
        public TimeSpan RetainAfterExpiry { get; set; } = TimeSpan.FromHours(1);

        /// <summary>
        /// Upper bound, in bytes of stored responses, for the in-memory store that
        /// <c>AddEsiCaching()</c> adds when the app has registered no
        /// <c>IDistributedCache</c> of its own. When it is full, new entries are not stored
        /// (those calls simply go to ESI) and older ones are evicted. Default 256 MB;
        /// <c>null</c> for no limit. Ignored when the app supplies its own store, which keeps
        /// whatever limits the app gave it.
        /// </summary>
        public long? InMemorySizeLimit { get; set; } = 256L * 1024 * 1024;
    }
}
