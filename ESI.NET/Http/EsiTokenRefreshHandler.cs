using ESI.NET.Models.SSO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ESI.NET.Http
{
    /// <summary>
    /// Transparently refreshes a near-expired access token before an authenticated request goes
    /// out. The character and any per-call callback are read from the request (put there by
    /// <see cref="EsiRequest"/>.Execute); after a refresh the request's bearer header is swapped,
    /// the per-call callback runs, and — when a DI scope is available — a registered
    /// <see cref="IEsiTokenRefreshSink"/> is invoked so the rotated refresh token can be persisted
    /// once, centrally.
    /// </summary>
    public sealed class EsiTokenRefreshHandler : DelegatingHandler
    {
        private static readonly TimeSpan Skew = TimeSpan.FromMinutes(1);

        private readonly EsiConfig _config;
        private readonly IServiceScopeFactory _scopeFactory;

        public EsiTokenRefreshHandler(IOptions<EsiConfig> config, IServiceScopeFactory scopeFactory = null)
        {
            _config = config.Value;
            _scopeFactory = scopeFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var character = EsiRequestState.GetCharacter(request);

            if (character != null
                && !string.IsNullOrEmpty(character.RefreshToken)
                && character.ExpiresOn != default
                && character.ExpiresOn <= DateTime.UtcNow.Add(Skew))
            {
                await SsoLogic.RefreshAccessTokenAsync((r, ct) => base.SendAsync(r, ct), _config, character, cancellationToken).ConfigureAwait(false);

                // the request was built with the stale token
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", character.Token);

                var perCall = EsiRequestState.GetCallback(request);
                if (perCall != null)
                    await perCall(character).ConfigureAwait(false);

                if (_scopeFactory != null)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var sink = scope.ServiceProvider.GetService<IEsiTokenRefreshSink>();
                        if (sink != null)
                            await sink.OnRefreshedAsync(character).ConfigureAwait(false);
                    }
                }
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
