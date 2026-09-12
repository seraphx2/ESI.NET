using System;

namespace ESI.NET.Http
{
    /// <summary>
    /// Thrown by <see cref="EsiErrorLimitHandler"/> when ESI responds with <c>420 Error Limited</c>.
    /// ESI enforces a rolling error budget per client IP; when it is exhausted every request is
    /// rejected until the window resets. See https://developers.eveonline.com/blog/article/error-limiting-imminent.
    /// </summary>
    public sealed class EsiErrorLimitException : Exception
    {
        public EsiErrorLimitException() { }

        public EsiErrorLimitException(string message) : base(message) { }

        public EsiErrorLimitException(string message, Exception innerException) : base(message, innerException) { }

        public EsiErrorLimitException(TimeSpan retryAfter)
            : base($"ESI error limit reached (HTTP 420). Retry after {retryAfter.TotalSeconds:0}s.")
        {
            RetryAfter = retryAfter;
        }

        /// <summary>How long until the error-limit window resets.</summary>
        public TimeSpan RetryAfter { get; }
    }
}
