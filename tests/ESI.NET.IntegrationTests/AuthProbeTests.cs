using System;
using System.Net;
using System.Threading.Tasks;
using ESI.NET;
using ESI.NET.Enumerations;
using ESI.NET.Models.SSO;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ESI.NET.IntegrationTests
{
    /// <summary>
    /// Exchanges the stored refresh token once, then reuses the result. If the
    /// credentials are not configured this does nothing and every probe test skips.
    /// </summary>
    public sealed class AuthFixture : IAsyncLifetime
    {
        public bool Enabled => LiveConfig.HasAuthConfig;
        public IEsiClient Client { get; private set; }
        public SsoToken Token { get; private set; }
        public AuthorizedCharacterData Character { get; private set; }

        public async Task InitializeAsync()
        {
            if (!Enabled)
                return;

            var services = new ServiceCollection();
            services.AddEsi(c =>
            {
                c.EsiUrl = "https://esi.evetech.net/";
                c.DataSource = LiveConfig.DataSource;
                c.UserAgent = LiveConfig.UserAgent;
                c.ClientId = LiveConfig.ClientId;
                c.SecretKey = LiveConfig.SecretKey;
            });
            Client = services.BuildServiceProvider().GetRequiredService<IEsiClient>();

            Token = await Client.SSO.GetToken(GrantType.RefreshToken, LiveConfig.RefreshToken);
            Character = await Client.SSO.Verify(Token);

            if (Token.RefreshToken != LiveConfig.RefreshToken)
                Console.WriteLine("::warning::ESI rotated the refresh token; update the ESI_REFRESH_TOKEN secret (re-run tools/MintToken).");
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }

    [CollectionDefinition("live-auth")]
    public sealed class LiveAuthCollection : ICollectionFixture<AuthFixture> { }

    /// <summary>
    /// The SSO / authenticated path end to end: token exchange, JWKS validation
    /// (the risky part of the IdentityModel 6-&gt;8 upgrade), a bearer call, and the
    /// transparent-refresh handler firing against live SSO. Skips unless
    /// ESI_CLIENT_ID + ESI_SECRET_KEY + ESI_REFRESH_TOKEN are set.
    ///
    /// The stored token needs the <c>esi-wallet.read_character_wallet.v1</c> scope
    /// (tools/MintToken's default).
    /// </summary>
    [Collection("live-auth")]
    public sealed class AuthProbeTests
    {
        private const string SkipReason = "ESI_CLIENT_ID / ESI_SECRET_KEY / ESI_REFRESH_TOKEN not set";
        private readonly AuthFixture _f;

        public AuthProbeTests(AuthFixture fixture) => _f = fixture;

        [SkippableFact]
        public void Refresh_token_exchange_returns_an_access_token()
        {
            Skip.IfNot(_f.Enabled, SkipReason);
            Assert.False(string.IsNullOrEmpty(_f.Token.AccessToken));
            Assert.False(string.IsNullOrEmpty(_f.Token.RefreshToken));
            Assert.True(_f.Token.ExpiresIn > 0);
        }

        [SkippableFact]
        public void Verify_projects_a_real_character()
        {
            Skip.IfNot(_f.Enabled, SkipReason);
            Assert.True(_f.Character.CharacterID > 0);
            Assert.False(string.IsNullOrEmpty(_f.Character.CharacterName));
            Assert.False(string.IsNullOrEmpty(_f.Character.CharacterOwnerHash));
        }

        [SkippableFact]
        public async Task Authenticated_call_succeeds()
        {
            Skip.IfNot(_f.Enabled, SkipReason);
            var r = await LiveFixture.Call(() => _f.Client.Wallet.CharacterWallet(new EsiCallOptions { Character = _f.Character }));
            Assert.Null(r.Exception);
            Assert.Equal(HttpStatusCode.OK, r.StatusCode);
        }

        [SkippableFact]
        public async Task Transparent_refresh_fires_on_a_stale_token()
        {
            Skip.IfNot(_f.Enabled, SkipReason);

            var stale = new AuthorizedCharacterData
            {
                CharacterID = _f.Character.CharacterID,
                CharacterName = _f.Character.CharacterName,
                CharacterOwnerHash = _f.Character.CharacterOwnerHash,
                Scopes = _f.Character.Scopes,
                Token = _f.Character.Token,
                RefreshToken = _f.Character.RefreshToken,
                ExpiresOn = DateTime.UtcNow.AddMinutes(-5), // force the handler to refresh
            };
            var tokenBefore = stale.Token;

            var r = await LiveFixture.Call(() => _f.Client.Wallet.CharacterWallet(new EsiCallOptions { Character = stale }));

            Assert.Null(r.Exception);
            Assert.Equal(HttpStatusCode.OK, r.StatusCode);
            Assert.NotEqual(tokenBefore, stale.Token); // EsiTokenRefreshHandler swapped the access token in place
        }
    }
}
