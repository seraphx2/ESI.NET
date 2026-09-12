using ESI.NET.Models.SSO;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ESI.NET
{
    /// <summary>
    /// Per-call options threaded through <see cref="EsiRequest"/>.Execute. Replaces the old
    /// <c>EsiClient.SetCharacterData</c> / <c>SetIfNoneMatchHeader</c> instance state, which was
    /// shared across every call on the client (and, for the ETag, across the whole process).
    /// </summary>
    public sealed class EsiCallOptions
    {
        /// <summary>
        /// The authorized character for endpoints that require SSO. Its <c>Token</c> is sent as the
        /// bearer token and its <c>CharacterID</c> fills the <c>{character_id}</c> path segment.
        /// Required for authenticated endpoints; ignored by public ones.
        /// </summary>
        public AuthorizedCharacterData Character { get; set; }

        /// <summary>Cancels the HTTP send and the response body read.</summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Sent as an <c>If-None-Match</c> header; a match yields <c>304 Not Modified</c> with no
        /// body. Pass the <see cref="EsiResponse{T}.ETag"/> from a previous response. Surrounding
        /// quotes are optional.
        /// </summary>
        public string IfNoneMatch { get; set; }

        /// <summary>1-based page for paginated endpoints; sent as <c>?page=</c>.</summary>
        public int? Page { get; set; }

        /// <summary>
        /// When set, an authenticated call whose <see cref="Character"/> access token is within a
        /// minute of expiry (or already expired) is transparently refreshed with its refresh token
        /// before the request is sent. <see cref="Character"/> is updated in place and this callback
        /// is invoked with it, so you can persist the rotated refresh token. Requires
        /// <c>EsiConfig.ClientId</c> (and <c>SecretKey</c> for a confidential client).
        /// </summary>
        public Func<AuthorizedCharacterData, Task> OnTokenRefreshed { get; set; }
    }
}
