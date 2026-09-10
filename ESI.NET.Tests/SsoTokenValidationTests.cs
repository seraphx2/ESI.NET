using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using ESI.NET.Models.SSO;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace ESI.NET.Tests
{
    /// <summary>
    /// Regression coverage for <c>SsoLogic.ValidateAccessToken</c> — the JWT-validation core of
    /// <c>SsoLogic.Verify</c>. The point is to prove the Microsoft.IdentityModel 6.x -> 8.x bump
    /// did not silently change how the access token is validated or how its claims are read.
    /// No EVE credentials and no network: tokens are signed with a throwaway RSA key, except the
    /// last test which parses a pinned snapshot of the real login.eveonline.com JWKS.
    /// </summary>
    public class SsoTokenValidationTests
    {
        private const string Host = "login.eveonline.com";
        private const string Issuer = "https://login.eveonline.com";
        private const string Kid = "JWT-Signature-Key";
        private const int CharacterId = 2112625428;

        private static readonly RSA SigningKey = RSA.Create(2048);

        private static string Jwks(RSA key = null)
        {
            var parameters = (key ?? SigningKey).ExportParameters(false);
            var jwk = new
            {
                kty = "RSA",
                use = "sig",
                alg = "RS256",
                kid = Kid,
                n = Base64UrlEncoder.Encode(parameters.Modulus),
                e = Base64UrlEncoder.Encode(parameters.Exponent),
            };
            return "{\"keys\":[" + JsonSerializer.Serialize(jwk) + "]}";
        }

        private static IEnumerable<Claim> DefaultClaims() => new[]
        {
            new Claim("sub", $"CHARACTER:EVE:{CharacterId}"),
            new Claim("name", "CCP Zoetrope"),
            new Claim("owner", "8PmzCeTKb4VFUDrHLc/n4VWtx1M="),
            new Claim("scp", "esi-skills.read_skills.v1"),
            new Claim("scp", "esi-wallet.read_character_wallet.v1"),
        };

        private static string SignToken(
            RSA key = null,
            string issuer = Issuer,
            DateTime? expires = null,
            IEnumerable<Claim> claims = null)
        {
            var signingKey = new RsaSecurityKey(key ?? SigningKey) { KeyId = Kid };
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256);
            var jwt = new JwtSecurityToken(
                issuer: issuer,
                claims: claims ?? DefaultClaims(),
                notBefore: DateTime.UtcNow.AddMinutes(-1),
                expires: expires ?? DateTime.UtcNow.AddMinutes(20),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static SsoToken Token(string accessToken) =>
            new SsoToken { AccessToken = accessToken, RefreshToken = "refresh-token-value" };

        [Fact]
        public void Valid_token_projects_identity_claims()
        {
            var result = SsoLogic.ValidateAccessToken(Token(SignToken()), Host, Jwks());

            Assert.Equal(CharacterId, result.CharacterID);
            Assert.Equal("CCP Zoetrope", result.CharacterName);
            Assert.Equal("8PmzCeTKb4VFUDrHLc/n4VWtx1M=", result.CharacterOwnerHash);
            Assert.Equal("esi-skills.read_skills.v1 esi-wallet.read_character_wallet.v1", result.Scopes);
            Assert.Equal("refresh-token-value", result.RefreshToken);
            Assert.True(result.ExpiresOn > DateTime.UtcNow);
        }

        [Fact]
        public void Claim_types_are_read_raw_not_remapped()
        {
            // Inbound claim-type mapping (sub -> schemas.xmlsoap.org/.../nameidentifier) is the most
            // likely silent breakage across the 6.x -> 8.x jump. Verify() reads "sub"/"name"/"owner"
            // /"scp" verbatim off the JwtSecurityToken, so finding the character id at all proves the
            // claim types survived unmapped.
            var result = SsoLogic.ValidateAccessToken(Token(SignToken()), Host, Jwks());
            Assert.Equal(CharacterId, result.CharacterID);
            Assert.NotEqual(0, result.CharacterID);
        }

        [Fact]
        public void Wrong_issuer_is_rejected()
        {
            var token = Token(SignToken(issuer: "https://login.evil.example"));
            Assert.ThrowsAny<SecurityTokenException>(() => SsoLogic.ValidateAccessToken(token, Host, Jwks()));
        }

        [Fact]
        public void Signature_from_a_different_key_is_rejected()
        {
            using var attacker = RSA.Create(2048);
            var forged = Token(SignToken(key: attacker));           // signed by the attacker's key...
            Assert.ThrowsAny<SecurityTokenException>(
                () => SsoLogic.ValidateAccessToken(forged, Host, Jwks()));  // ...validated against the real JWKS
        }

        [Fact]
        public void Expired_beyond_clock_skew_is_rejected()
        {
            var token = Token(SignToken(expires: DateTime.UtcNow.AddSeconds(-30)));
            Assert.Throws<SecurityTokenExpiredException>(() => SsoLogic.ValidateAccessToken(token, Host, Jwks()));
        }

        [Fact]
        public void Expired_within_clock_skew_still_validates()
        {
            // ValidateAccessToken allows a 2s skew for CCP's slightly-fast clocks.
            var token = Token(SignToken(expires: DateTime.UtcNow.AddSeconds(-1)));
            var result = SsoLogic.ValidateAccessToken(token, Host, Jwks());
            Assert.Equal(CharacterId, result.CharacterID);
        }

        [Fact]
        public void Pinned_real_eve_jwks_parses_under_current_IdentityModel()
        {
            // Snapshot of https://login.eveonline.com/oauth/jwks (public keys only). Proves the real
            // payload shape — including CCP's non-standard "SkipUnresolvedJsonWebKeys" field and the
            // trailing EC key — still round-trips through JsonWebKeySet, and that Keys.First() (what
            // Verify() takes) is the RS256 signing key.
            var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "login.eveonline.com-jwks.json"));

            var set = new JsonWebKeySet(json);

            Assert.Equal(2, set.Keys.Count);
            var first = set.Keys.First();
            Assert.Equal(Kid, first.Kid);
            Assert.Equal("RSA", first.Kty);
            Assert.Equal("AQAB", first.E);
        }
    }
}
