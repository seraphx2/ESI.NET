using ESI.NET.Models.SSO;
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
    }
}
