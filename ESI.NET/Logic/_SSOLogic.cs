using ESI.NET.Enumerations;
using ESI.NET.Models.Character;
using ESI.NET.Models.SSO;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ESI.NET
{
    public class SsoLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly string _clientKey;
        private readonly string _ssoUrl;

        // Cryptographically secure: the PKCE code_verifier's entire protection depends on being
        // unpredictable (RFC 7636). System.Random is seeded and predictable and must not be used here.
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        public SsoLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
            switch (_config.DataSource)
            {
                case DataSource.Tranquility:
                    _ssoUrl = "login.eveonline.com";
                    break;
                case DataSource.Serenity:
                    _ssoUrl = "login.evepc.163.com";
                    break;
            }
            _clientKey = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{config.ClientId}:{config.SecretKey}"));
        }

        /// <summary>The SSO host for the configured <see cref="DataSource"/>.</summary>
        internal static string SsoHost(DataSource dataSource)
        {
            switch (dataSource)
            {
                case DataSource.Serenity: return "login.evepc.163.com";
                default: return "login.eveonline.com";
            }
        }

        /// <summary>
        /// POSTs <paramref name="requestBody"/> to the SSO <c>/v2/oauth/token</c> endpoint. Uses HTTP
        /// Basic auth when <see cref="EsiConfig.SecretKey"/> is set (confidential client); otherwise
        /// the caller is expected to have put <c>client_id</c> in the body (PKCE client).
        /// </summary>
        internal static async Task<SsoToken> RequestTokenAsync(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> send, EsiConfig config, string requestBody, CancellationToken cancellationToken = default)
        {
            var host = SsoHost(config.DataSource);
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{host}/v2/oauth/token")
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/x-www-form-urlencoded"),
            };

            if (!string.IsNullOrEmpty(config.SecretKey))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{config.ClientId}:{config.SecretKey}")));
                request.Headers.Host = host;
            }

            using (var response = await send(request, cancellationToken).ConfigureAwait(false))
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new ArgumentException(JsonConvert.DeserializeAnonymousType(content, new { error_description = string.Empty }).error_description);
                return JsonConvert.DeserializeObject<SsoToken>(content);
            }
        }

        /// <summary>
        /// Exchanges <paramref name="character"/>'s refresh token for a new access token and updates
        /// <see cref="AuthorizedCharacterData.Token"/>, <see cref="AuthorizedCharacterData.RefreshToken"/>
        /// (EVE rotates it) and <see cref="AuthorizedCharacterData.ExpiresOn"/> in place.
        /// </summary>
        internal static async Task RefreshAccessTokenAsync(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> send, EsiConfig config, AuthorizedCharacterData character, CancellationToken cancellationToken = default)
        {
            var body = $"grant_type={GrantType.RefreshToken.ToEsiValue()}&refresh_token={Uri.EscapeDataString(character.RefreshToken)}";
            if (string.IsNullOrEmpty(config.SecretKey))
                body += $"&client_id={config.ClientId}";

            var token = await RequestTokenAsync(send, config, body, cancellationToken).ConfigureAwait(false);

            character.Token = token.AccessToken;
            character.RefreshToken = token.RefreshToken;
            character.ExpiresOn = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="state"></param>
        /// <param name="code_challenge">All hashing/encryption will be done automatically. Just provide the code.</param>
        /// <param name=""></param>
        /// <returns></returns>
        public string CreateAuthenticationUrl(List<string> scope = null, string state = null, string challengeCode = null)
        {
            var url = $"https://{_ssoUrl}/v2/oauth/authorize/?response_type=code&redirect_uri={Uri.EscapeDataString(_config.CallbackUrl)}&client_id={_config.ClientId}";

            if (scope != null)
                url = $"{url}&scope={string.Join("+", scope.Distinct().ToList())}";

            if (state != null)
                url = $"{url}&state={state}";

            if (challengeCode != null)
            {
                url = $"{url}&code_challenge_method=S256";

                using (var sha256 = SHA256.Create())
                {
                    var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(challengeCode)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
                    var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(base64));
                    var code_challenge = Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');

                    url = $"{url}&code_challenge={code_challenge}";
                }
            }

            return url;
        }
        
        public string GenerateChallengeCode()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            // Reject-and-retry on bytes past the last full multiple of chars.Length (248 for 62
            // chars), instead of a plain modulo, so every character is exactly equally likely.
            var limit = (byte)(256 - 256 % chars.Length);
            var result = new char[32];
            var buffer = new byte[1];
            for (var i = 0; i < result.Length; i++)
            {
                byte b;
                do
                {
                    Rng.GetBytes(buffer);
                    b = buffer[0];
                } while (b >= limit);
                result[i] = chars[b % chars.Length];
            }
            return new string(result);
        }

        /// <summary>
        /// SSO Token helper
        /// </summary>
        /// <param name="grantType"></param>
        /// <param name="code">The authorization_code or the refresh_token</param>
        /// <param name="codeChallenge">Provide the same value that was provided for codeChallenge in CreateAuthenticationUrl(). All hashing/encryption will be done automatically. Just provide the code.</param>
        /// <returns></returns>
        public async Task<SsoToken> GetToken(GrantType grantType, string code, string codeChallenge = null)
        {
            var body = $"grant_type={grantType.ToEsiValue()}";
            if (grantType == GrantType.AuthorizationCode)
            {
                body += $"&code={code}";

                if (codeChallenge != null)
                {
                    var bytes = Encoding.ASCII.GetBytes(codeChallenge);
                    var base64 = Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
                    body += $"&code_verifier={base64}&client_id={_config.ClientId}";
                }
            }   
            else if (grantType == GrantType.RefreshToken)
            {
                body += $"&refresh_token={Uri.EscapeDataString(code)}";

                if(codeChallenge != null)
                    body += $"&client_id={_config.ClientId}";
            }

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{_ssoUrl}/v2/oauth/token")
            {
                Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded"),
            };
            if(codeChallenge == null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _clientKey);
                request.Headers.Host = _ssoUrl;
            }

            var response = await _client.SendAsync(request).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeAnonymousType(content, new { error_description = string.Empty }).error_description;
                throw new ArgumentException(error);
            }

            var token = JsonConvert.DeserializeObject<SsoToken>(content);

            return token;
        }

        /// <summary>
        /// SSO Token revokation helper
        /// ESI will invalidate the provided refreshToken
        /// </summary>
        /// <param name="code">refresh_token to revoke</param>
        /// <returns></returns>
        public async Task RevokeToken(string code)
        {
            var body = $"token_type_hint={GrantType.RefreshToken.ToEsiValue()}";
            body += $"&token={Uri.EscapeDataString(code)}";

            HttpContent postBody = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _clientKey);

            var response = await _client.PostAsync($"https://{_ssoUrl}/v2/oauth/revoke", postBody).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeAnonymousType(content, new { error_description = string.Empty }).error_description;
                throw new ArgumentException(error);
            }
        }

        /// <summary>
        /// Validates <paramref name="token"/>'s access token against the SSO JWKS and projects the
        /// identity claims (character id, name, owner hash, scopes, expiry) onto a fresh
        /// <see cref="AuthorizedCharacterData"/>. Throws if the token fails validation.
        /// Split out of <see cref="Verify"/> so the validation path can be exercised without a
        /// live SSO endpoint. The affiliation lookup stays in <see cref="Verify"/>.
        /// </summary>
        /// <param name="clientId">
        /// This application's own OAuth client ID. Per CCP's documented JWT validation requirements
        /// (docs.esi.evetech.net/docs/sso/validating_eve_jwt.html), a real EVE SSO access token's
        /// <c>aud</c> claim is <c>[clientId, "EVE Online"]</c>, and both are required to be present -
        /// this rejects a token that is well-formed and correctly signed by CCP but was issued for a
        /// *different* registered application (client-confusion / cross-app token replay).
        /// </param>
        internal static AuthorizedCharacterData ValidateAccessToken(SsoToken token, string ssoUrl, string clientId, string jwksJson)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwks = new JsonWebKeySet(jwksJson);
            var jwk = jwks.Keys.First();

            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateAudience = true,
                AudienceValidator = (audiences, _, _) =>
                    audiences.Contains("EVE Online", StringComparer.Ordinal) && audiences.Contains(clientId, StringComparer.Ordinal),
                ValidateIssuer = true,
                ValidIssuer = $"https://{ssoUrl}",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = jwk,
                ClockSkew = TimeSpan.FromSeconds(2), // CCP's servers seem slightly ahead (~1s)
            };
            tokenHandler.ValidateToken(token.AccessToken, tokenValidationParams, out var validatedToken);

            var jwt = (JwtSecurityToken)validatedToken;

            var subjectClaim = jwt.Claims.SingleOrDefault(c => c.Type == "sub").Value;
            var nameClaim = jwt.Claims.SingleOrDefault(c => c.Type == "name").Value;
            var ownerClaim = jwt.Claims.SingleOrDefault(c => c.Type == "owner").Value;
            var scopesClaim = string.Join(" ", jwt.Claims.Where(c => c.Type == "scp").Select(s => s.Value));

            return new AuthorizedCharacterData
            {
                RefreshToken = token.RefreshToken,
                Token = token.AccessToken,
                CharacterName = nameClaim,
                CharacterOwnerHash = ownerClaim,
                CharacterID = long.Parse(subjectClaim.Split(':').Last(), CultureInfo.InvariantCulture),
                ExpiresOn = jwt.ValidTo,
                Scopes = scopesClaim,
            };
        }

        /// <summary>
        /// Validates <paramref name="token"/>'s access token against the EVE SSO JWKS and returns an
        /// <see cref="AuthorizedCharacterData"/> carrying the character identity, the granted scopes,
        /// the token/refresh token, and (best-effort) the current alliance/corporation/faction.
        /// Persist this per character; you need at least <c>RefreshToken</c> and
        /// <c>CharacterOwnerHash</c> for the long term.
        /// </summary>
        /// <remarks>
        /// Compare <see cref="AuthorizedCharacterData.CharacterOwnerHash"/> against your stored value
        /// on every re-login: EVE reissues it when a character is transferred to another account, and
        /// a mismatch means the stored token/data belongs to a previous owner and must be discarded.
        /// </remarks>
        /// <exception cref="InvalidOperationException">The access token failed validation.</exception>
        public async Task<AuthorizedCharacterData> Verify(SsoToken token)
        {
            AuthorizedCharacterData authorizedCharacter;

            try
            {
                // Get the EVE Online JWKS to validate the access token against
                var jwksUrl = $"https://{_ssoUrl}/oauth/jwks";
                string jwksJson;
                using (var jwksResponse = await _client.GetAsync(jwksUrl).ConfigureAwait(false))
                    jwksJson = await jwksResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                authorizedCharacter = ValidateAccessToken(token, _ssoUrl, _config.ClientId, jwksJson);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "SSO access-token verification failed. The token may be expired, malformed, issued for a " +
                    "different SSO host, or issued for a different application (client_id mismatch).", ex);
            }

            // Best-effort enrichment: a failure here does not invalidate the token.
            try
            {
                var url = $"{_config.EsiUrl.TrimEnd('/')}/characters/affiliation/";
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new[] { authorizedCharacter.CharacterID }), Encoding.UTF8, "application/json"),
                };
                request.Headers.Add("X-Compatibility-Date", EsiVersion.CompatibilityDate);
                request.Headers.Add("X-Tenant", _config.DataSource.ToEsiValue());

                var affiliationResponse = await _client.SendAsync(request).ConfigureAwait(false);
                var affiliations = await EsiResponse<List<Affiliation>>.CreateAsync(affiliationResponse, "Post|/character/affiliations/").ConfigureAwait(false);

                if (affiliations.StatusCode == HttpStatusCode.OK && affiliations.Data?.Count > 0)
                {
                    var characterData = affiliations.Data.First();
                    authorizedCharacter.AllianceID = characterData.AllianceId;
                    authorizedCharacter.CorporationID = characterData.CorporationId;
                    authorizedCharacter.FactionID = characterData.FactionId;
                }
            }
            catch
            {
                // affiliation enrichment is best-effort
            }

            return authorizedCharacter;
        }
    }
}
