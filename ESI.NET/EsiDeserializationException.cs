using System;
using System.Net;

namespace ESI.NET
{
    /// <summary>
    /// Thrown when ESI answered a request successfully (<c>200</c>/<c>201</c>) but the
    /// body could not be read into the expected model.
    /// <para>
    /// This means ESI.NET and ESI disagree about the response shape - a library bug
    /// or unannounced drift, not an outcome a caller is expected to handle - so it is
    /// thrown rather than returned as an <see cref="EsiResponse{T}"/> with a null
    /// <c>Data</c> that would look like success. Please report it with
    /// <see cref="Endpoint"/>, <see cref="RequestId"/> and the inner exception.
    /// </para>
    /// </summary>
    public sealed class EsiDeserializationException : Exception
    {
        /// <summary>The longest slice of the body kept on <see cref="Body"/>.</summary>
        public const int MaxBodyLength = 4096;

        public EsiDeserializationException() { }

        public EsiDeserializationException(string message) : base(message) { }

        public EsiDeserializationException(string message, Exception innerException) : base(message, innerException) { }

        internal EsiDeserializationException(
            string endpoint, HttpStatusCode statusCode, Guid requestId, Type targetType, string body, Exception innerException)
            : base($"ESI returned {(int)statusCode} for {endpoint}, but the body could not be read as {targetType}: {innerException.Message}", innerException)
        {
            Endpoint = endpoint;
            StatusCode = statusCode;
            RequestId = requestId;
            TargetType = targetType;
            Body = body != null && body.Length > MaxBodyLength ? body.Substring(0, MaxBodyLength) : body;
        }

        /// <summary>The ESI route template, e.g. <c>/characters/{character_id}/</c>.</summary>
        public string Endpoint { get; }

        /// <summary>The (successful) status ESI returned.</summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>ESI's <c>X-ESI-Request-ID</c>, or <see cref="Guid.Empty"/> if it sent none.</summary>
        public Guid RequestId { get; }

        /// <summary>The model the body was being read into.</summary>
        public Type TargetType { get; }

        /// <summary>The response body, cut to <see cref="MaxBodyLength"/> characters.</summary>
        public string Body { get; }
    }
}
