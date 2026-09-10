using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET;
using ESI.NET.Enumerations;
using ESI.NET.Models.SSO;
using Xunit;
using static ESI.NET.EsiRequest;

namespace ESI.NET.Tests
{
    /// <summary>
    /// Transparent access-token refresh: when <see cref="EsiCallOptions.OnTokenRefreshed"/> is set
    /// and the character's token is (near) expired, <see cref="EsiRequest"/>.Execute exchanges the
    /// refresh token before sending, updates the character in place, and invokes the callback.
    /// </summary>
    public class TokenRefreshTests
    {
        private sealed class RoutingHandler : HttpMessageHandler
        {
            public HttpRequestMessage LastApiRequest;
            public int TokenCalls;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (request.RequestUri.AbsolutePath == "/v2/oauth/token")
                {
                    TokenCalls++;
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(
                            @"{ ""access_token"": ""NEW-ACCESS"", ""token_type"": ""Bearer"", ""expires_in"": 1200, ""refresh_token"": ""NEW-REFRESH"" }"),
                    });
                }

                LastApiRequest = request;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{}") });
            }
        }

        private static readonly EsiConfig Config = new EsiConfig
        {
            EsiUrl = "https://esi.evetech.net/",
            DataSource = DataSource.Tranquility,
            ClientId = "client-id",
            SecretKey = "client-secret",
        };

        private static AuthorizedCharacterData Character(DateTime expiresOn) => new AuthorizedCharacterData
        {
            CharacterID = 42,
            Token = "OLD-ACCESS",
            RefreshToken = "OLD-REFRESH",
            ExpiresOn = expiresOn,
        };

        [Fact]
        public async Task Expired_token_is_refreshed_and_the_callback_fires()
        {
            var handler = new RoutingHandler();
            var client = new HttpClient(handler);
            var character = Character(DateTime.UtcNow.AddMinutes(-5));

            AuthorizedCharacterData callbackArg = null;
            var options = new EsiCallOptions
            {
                Character = character,
                OnTokenRefreshed = c => { callbackArg = c; return Task.CompletedTask; },
            };

            await Execute<object>(client, Config, RequestSecurity.Authenticated, HttpMethod.Get, "/x/", options: options);

            Assert.Equal(1, handler.TokenCalls);
            Assert.Equal("NEW-ACCESS", character.Token);
            Assert.Equal("NEW-REFRESH", character.RefreshToken);
            Assert.True(character.ExpiresOn > DateTime.UtcNow.AddMinutes(15));
            Assert.Same(character, callbackArg);
            // the actual API call carried the refreshed token
            Assert.Equal("NEW-ACCESS", handler.LastApiRequest.Headers.Authorization.Parameter);
        }

        [Fact]
        public async Task A_still_valid_token_is_not_refreshed()
        {
            var handler = new RoutingHandler();
            var client = new HttpClient(handler);
            var character = Character(DateTime.UtcNow.AddMinutes(10));

            var fired = false;
            var options = new EsiCallOptions
            {
                Character = character,
                OnTokenRefreshed = _ => { fired = true; return Task.CompletedTask; },
            };

            await Execute<object>(client, Config, RequestSecurity.Authenticated, HttpMethod.Get, "/x/", options: options);

            Assert.Equal(0, handler.TokenCalls);
            Assert.False(fired);
            Assert.Equal("OLD-ACCESS", handler.LastApiRequest.Headers.Authorization.Parameter);
        }

        [Fact]
        public async Task Without_the_callback_an_expired_token_is_left_alone()
        {
            var handler = new RoutingHandler();
            var client = new HttpClient(handler);
            var character = Character(DateTime.UtcNow.AddMinutes(-5));

            var options = new EsiCallOptions { Character = character }; // no OnTokenRefreshed

            await Execute<object>(client, Config, RequestSecurity.Authenticated, HttpMethod.Get, "/x/", options: options);

            Assert.Equal(0, handler.TokenCalls);
            Assert.Equal("OLD-ACCESS", handler.LastApiRequest.Headers.Authorization.Parameter);
        }
    }
}
