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
        /// <summary>Creates an exception with a default message.</summary>
        public EsiErrorLimitException() { }

        /// <summary>Creates an exception with <paramref name="message"/>.</summary>
        /// <param name="message">The error message.</param>
        public EsiErrorLimitException(string message) : base(message) { }

        /// <summary>Creates an exception with <paramref name="message"/> and the exception that caused it.</summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The exception that caused this one.</param>
        public EsiErrorLimitException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>Creates the exception for a <c>420</c>, with how long to wait.</summary>
        /// <param name="retryAfter">How long until the error-limit window resets.</param>
        public EsiErrorLimitException(TimeSpan retryAfter)
            : base($"ESI error limit reached (HTTP 420). Retry after {retryAfter.TotalSeconds:0}s.")
        {
            RetryAfter = retryAfter;
        }

        /// <summary>How long until the error-limit window resets.</summary>
        public TimeSpan RetryAfter { get; }
    }
}
