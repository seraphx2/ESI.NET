using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET;
using ESI.NET.Enumerations;
using ESI.NET.Http;
using ESI.NET.Models.SSO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace ESI.NET.Tests
{
    /// <summary>
    /// Transparent access-token refresh, done by <see cref="EsiTokenRefreshHandler"/>: a near-expired
    /// token is exchanged before the request goes out, the character is updated in place, the bearer
    /// header is swapped, and the per-call callback and/or a registered
    /// <see cref="IEsiTokenRefreshSink"/> are invoked.
    /// </summary>
    public class TokenRefreshTests
    {
        private const string NewTokenJson =
            @"{ ""access_token"": ""NEW-ACCESS"", ""token_type"": ""Bearer"", ""expires_in"": 1200, ""refresh_token"": ""NEW-REFRESH"" }";

        private const string InvalidGrantJson = @"{ ""error"": ""invalid_grant"", ""error_description"": ""The refresh token is invalid or expired."" }";

        private sealed class RoutingHandler : HttpMessageHandler
        {
            public HttpRequestMessage LastApiRequest;
            public int TokenCalls;
            public string ApiBody = "{}";

            /// <summary>When set, the token endpoint returns this instead of a fresh token — simulates EVE rejecting the refresh (revoked/expired/rescoped).</summary>
            public HttpStatusCode? TokenFailureStatus;
            public string TokenFailureBody = InvalidGrantJson;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (request.RequestUri.AbsolutePath == "/v2/oauth/token")
                {
                    TokenCalls++;
                    return Task.FromResult(TokenFailureStatus.HasValue
                        ? new HttpResponseMessage(TokenFailureStatus.Value) { Content = new StringContent(TokenFailureBody) }
                        : new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(NewTokenJson) });
                }

                LastApiRequest = request;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(ApiBody) });
            }
        }

        private sealed class FakeSink : IEsiTokenRefreshSink
        {
            public AuthorizedCharacterData Received;
            public AuthorizedCharacterData FailedCharacter;
            public Exception FailedException;
            public Task OnRefreshedAsync(AuthorizedCharacterData character) { Received = character; return Task.CompletedTask; }
            public Task OnRefreshFailedAsync(AuthorizedCharacterData character, Exception exception) { FailedCharacter = character; FailedException = exception; return Task.CompletedTask; }
        }

        private static readonly EsiConfig Config = new EsiConfig
        {
            EsiUrl = "https://esi.evetech.net/",
            DataSource = DataSource.Tranquility,
            ClientId = "client-id",
            SecretKey = "client-secret",
            UserAgent = "refresh-tests",
        };

        private static AuthorizedCharacterData Character(DateTime expiresOn) => new AuthorizedCharacterData
        {
            CharacterID = 42,
            Token = "OLD-ACCESS",
            RefreshToken = "OLD-REFRESH",
            ExpiresOn = expiresOn,
        };

        private static HttpRequestMessage AuthedRequest(AuthorizedCharacterData character, Func<AuthorizedCharacterData, Task> perCall = null)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "https://esi.evetech.net/latest/x/");
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", character.Token);
            EsiRequestState.SetCharacter(req, character);
            if (perCall != null) EsiRequestState.SetCallback(req, perCall);
            return req;
        }

        private static async Task SendThrough(EsiTokenRefreshHandler handler, HttpMessageHandler inner, HttpRequestMessage request)
        {
            handler.InnerHandler = inner;
            using var invoker = new HttpMessageInvoker(handler);
            await invoker.SendAsync(request, default);
        }

        [Fact]
        public async Task Expired_token_is_refreshed_before_the_request()
        {
            var routing = new RoutingHandler();
            var character = Character(DateTime.UtcNow.AddMinutes(-5));
            var handler = new EsiTokenRefreshHandler(Options.Create(Config));

            await SendThrough(handler, routing, AuthedRequest(character));

            Assert.Equal(1, routing.TokenCalls);
            Assert.Equal("NEW-ACCESS", character.Token);
            Assert.Equal("NEW-REFRESH", character.RefreshToken);
            Assert.True(character.ExpiresOn > DateTime.UtcNow.AddMinutes(15));
            Assert.Equal("NEW-ACCESS", routing.LastApiRequest.Headers.Authorization.Parameter);
        }

        [Fact]
        public async Task A_still_valid_token_is_left_alone()
        {
            var routing = new RoutingHandler();
            var character = Character(DateTime.UtcNow.AddMinutes(10));
            var handler = new EsiTokenRefreshHandler(Options.Create(Config));

            await SendThrough(handler, routing, AuthedRequest(character));

            Assert.Equal(0, routing.TokenCalls);
            Assert.Equal("OLD-ACCESS", routing.LastApiRequest.Headers.Authorization.Parameter);
        }

        [Fact]
        public async Task Per_call_callback_fires_on_refresh()
        {
            var routing = new RoutingHandler();
            var character = Character(DateTime.UtcNow.AddSeconds(-1));
            AuthorizedCharacterData got = null;
            var handler = new EsiTokenRefreshHandler(Options.Create(Config));

            await SendThrough(handler, routing, AuthedRequest(character, c => { got = c; return Task.CompletedTask; }));

            Assert.Same(character, got);
        }

        [Fact]
        public async Task Registered_sink_fires_on_refresh()
        {
            var sink = new FakeSink();
            var provider = new ServiceCollection().AddSingleton<IEsiTokenRefreshSink>(sink).BuildServiceProvider();
            var routing = new RoutingHandler();
            var character = Character(DateTime.UtcNow.AddMinutes(-5));
            var handler = new EsiTokenRefreshHandler(Options.Create(Config), provider.GetRequiredService<IServiceScopeFactory>());

            await SendThrough(handler, routing, AuthedRequest(character));

            Assert.Same(character, sink.Received);
        }

        [Fact]
        public async Task Failed_refresh_notifies_the_sink_and_still_throws()
        {
            var sink = new FakeSink();
            var provider = new ServiceCollection().AddSingleton<IEsiTokenRefreshSink>(sink).BuildServiceProvider();
            var routing = new RoutingHandler { TokenFailureStatus = HttpStatusCode.BadRequest };
            var character = Character(DateTime.UtcNow.AddMinutes(-5));
            var handler = new EsiTokenRefreshHandler(Options.Create(Config), provider.GetRequiredService<IServiceScopeFactory>());

            await Assert.ThrowsAsync<ArgumentException>(() => SendThrough(handler, routing, AuthedRequest(character)));

            Assert.Same(character, sink.FailedCharacter);
            Assert.NotNull(sink.FailedException);
            Assert.Null(sink.Received); // OnRefreshedAsync must not fire on a failed refresh
            Assert.Equal("OLD-ACCESS", character.Token); // unchanged - the failed exchange never got to mutate it
            Assert.Equal("OLD-REFRESH", character.RefreshToken);
        }

        [Fact]
        public async Task AddEsi_plus_one_sink_registration_covers_every_authenticated_call()
        {
            var routing = new RoutingHandler { ApiBody = "{}" };
            var sink = new FakeSink();

            var services = new ServiceCollection();
            services.AddSingleton<IEsiTokenRefreshSink>(sink);
            services.AddEsi(c =>
            {
                c.EsiUrl = "https://esi.evetech.net/";
                c.DataSource = DataSource.Tranquility;
                c.ClientId = "id";
                c.SecretKey = "secret";
                c.UserAgent = "e2e";
            }).ConfigurePrimaryHttpMessageHandler(() => routing);

            var client = services.BuildServiceProvider().GetRequiredService<IEsiClient>();
            var character = Character(DateTime.UtcNow.AddMinutes(-5));

            await client.Clones.List(new EsiCallOptions { Character = character });

            Assert.Equal(1, routing.TokenCalls);
            Assert.Same(character, sink.Received);
            Assert.Equal("NEW-ACCESS", routing.LastApiRequest.Headers.Authorization.Parameter);
            Assert.Contains("esi.evetech.net/characters/42/clones/", routing.LastApiRequest.RequestUri.ToString());
        }
    }
}
