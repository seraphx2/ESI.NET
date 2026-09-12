using ESI.NET.Models.SSO;
using System;
using System.Threading.Tasks;

namespace ESI.NET.Http
{
    /// <summary>
    /// Register one implementation (<c>services.AddScoped&lt;IEsiTokenRefreshSink, YourSink&gt;()</c>)
    /// and every authenticated call that transparently refreshes an access token will hand the
    /// updated <see cref="AuthorizedCharacterData"/> to it — one place to persist the rotated
    /// refresh token, instead of an <c>OnTokenRefreshed</c> callback on every call.
    /// </summary>
    public interface IEsiTokenRefreshSink
    {
        /// <summary>
        /// Called after a refresh, with <paramref name="character"/>'s <c>Token</c>,
        /// <c>RefreshToken</c> and <c>ExpiresOn</c> already updated in place.
        /// </summary>
        Task OnRefreshedAsync(AuthorizedCharacterData character);

        /// <summary>
        /// Called when a refresh attempt itself throws (the refresh token was revoked, expired, or
        /// scopes changed). <paramref name="character"/> is unchanged from before the attempt.
        /// The triggering call still fails — this is fired just before that exception propagates —
        /// so use it to react (e.g. flag the character as needing re-authorization), not to recover.
        /// </summary>
        Task OnRefreshFailedAsync(AuthorizedCharacterData character, Exception exception);
    }
}
