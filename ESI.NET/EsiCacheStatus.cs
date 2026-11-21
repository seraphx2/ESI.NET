namespace ESI.NET
{
    /// <summary>Where an <see cref="EsiResponse{T}"/> came from when the response cache is on.</summary>
    public enum EsiCacheStatus
    {
        /// <summary>
        /// The cache was not involved: caching is off, the request was not a <c>GET</c>, or the
        /// call set its own <see cref="EsiCallOptions.IfNoneMatch"/>.
        /// </summary>
        None = 0,

        /// <summary>
        /// Fetched from ESI: nothing usable was cached, ESI sent a changed body, or the response
        /// can't be cached (an error, a <c>no-store</c>). With <see cref="EsiCallOptions.BypassCache"/>
        /// the result is this or <see cref="Revalidated"/>.
        /// </summary>
        Miss,

        /// <summary>Served from the cache without contacting ESI; ESI's <c>Expires</c> had not passed.</summary>
        Hit,

        /// <summary>
        /// The cached copy had expired; ESI confirmed it unchanged (<c>304</c>) and the cached body
        /// was returned with refreshed cache headers.
        /// </summary>
        Revalidated,
    }
}
