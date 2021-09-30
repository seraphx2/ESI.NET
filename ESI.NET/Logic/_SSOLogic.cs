using ESI.NET.Enumerations;
using ESI.NET.Models.Character;
using ESI.NET.Models.SSO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ESI.NET
{
    public class SsoLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly string _clientKey;
        private readonly string _ssoUrl;

        public SsoLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;
            switch (_config.DataSource)
            {
                case DataSource.Tranquility:
                    _ssoUrl = "https://login.eveonline.com";
                    break;
                case DataSource.Serenity:
                    _ssoUrl = "https://login.evepc.163.com";
                    break;
            }
            _clientKey = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{config.ClientId}:{config.SecretKey}"));
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
            string authVersion = string.Empty;

            switch (_config.AuthVersion)
            {
                case AuthVersion.v2:
                    authVersion = "/v2";
                    break;
            }

            var url = $"{_ssoUrl}{authVersion}/oauth/authorize/?response_type=code&redirect_uri={Uri.EscapeDataString(_config.CallbackUrl)}&client_id={_config.ClientId}";

            if (scope != null)
                url = $"{url}&scope={string.Join("+", scope.Distinct().ToList())}";

            if (state != null)
                url = $"{url}&state={state}";

            if (challengeCode != null)
            {
                url = $"{url}&code_challenge_method=S256";

                var code_challenge = string.Empty;

                using (var sha256 = SHA256.Create())
                {
                    var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(challengeCode)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
                    var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(base64));
                    code_challenge = Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');

                    url = $"{url}&code_challenge={code_challenge}";
                }
            }

            return url;
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

                // if we have no Secret Key we're using PCKE so need to pass the client_id direct
                if(string.IsNullOrEmpty(_config.SecretKey))
                {
                    body += $"&client_id={_config.ClientId}";
                }
            }

            HttpContent postBody = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
            if(!String.IsNullOrEmpty(_config.SecretKey))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _clientKey);
            }

            HttpResponseMessage responseBase = null;

            if (_config.AuthVersion == AuthVersion.v1)
                responseBase = await _client.PostAsync($"{_ssoUrl}/oauth/token", postBody);
            else if (_config.AuthVersion == AuthVersion.v2)
                responseBase = await _client.PostAsync($"{_ssoUrl}/v2/oauth/token", postBody);

            var response = await responseBase.Content.ReadAsStringAsync();

            if (responseBase.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeAnonymousType(response, new { error_description = string.Empty }).error_description;
                throw new ArgumentException(error);
            }

            var token = JsonConvert.DeserializeObject<SsoToken>(response);

            return token;
        }

        /// <summary>
        /// Verifies the Character information for the provided Token information.
        /// While this method represents the oauth/verify request, in addition to the verified data that ESI returns, this object also stores the Token and Refresh token
        /// and this method also uses ESI retrieves other information pertinent to making calls in the ESI.NET API. (alliance_id, corporation_id, faction_id)
        /// You will need a record in your database that stores at least this information. Serialize and store this object for quick retrieval and token refreshing.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AuthorizedCharacterData> Verify(SsoToken token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            var response = await _client.GetAsync($"{_ssoUrl}/oauth/verify").Result.Content.ReadAsStringAsync();
            var authorizedCharacter = JsonConvert.DeserializeObject<AuthorizedCharacterData>(response);
            authorizedCharacter.Token = token.AccessToken;
            authorizedCharacter.RefreshToken = token.RefreshToken;

            var url = $"{_config.EsiUrl}v1/characters/affiliation/?datasource={_config.DataSource.ToEsiValue()}";
            var body = new StringContent(JsonConvert.SerializeObject(new int[] { authorizedCharacter.CharacterID }), Encoding.UTF8, "application/json");

            // Get more specifc details about authorized character to be used in API calls that require this data about the character
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            var characterResponse = await client.PostAsync(url, body).ConfigureAwait(false);

            if (characterResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                EsiResponse<List<Affiliation>> affiliations = new EsiResponse<List<Affiliation>>(characterResponse, "Post|/character/affiliations/", "v1");
                var characterData = affiliations.Data.First();

                authorizedCharacter.AllianceID = characterData.AllianceId;
                authorizedCharacter.CorporationID = characterData.CorporationId;
                authorizedCharacter.FactionID = characterData.FactionId;
            }

            return authorizedCharacter;
        }
    }
}
