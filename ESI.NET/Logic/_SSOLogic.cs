using ESI.NET.Enumerations;
using ESI.NET.Models.Character;
using ESI.NET.Models.SSO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public string CreateAuthenticationUrl(List<string> scopes = null, string state = "")
        {
            string authVersion = string.Empty;

            switch (_config.AuthVersion)
            {
                case AuthVersion.v2:
                    authVersion = "/v2";
                    break;
            }

            return $"{_ssoUrl}{authVersion}/oauth/authorize/?response_type=code&redirect_uri={Uri.EscapeDataString(_config.CallbackUrl)}&client_id={_config.ClientId}{((scopes != null) ? $"&scope={string.Join(" ", scopes)}" : "")}{((state != null) ? $"&state={state}" : "")}";
        }

        /// <summary>
        /// SSO Token helper
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="secretKey"></param>
        /// <param name="grantType"></param>
        /// <param name="code">The authorization_code or the refresh_token</param>
        /// <returns></returns>
        public async Task<SsoToken> GetToken(GrantType grantType, string code)
        {
            var body = $"grant_type={grantType.ToEsiValue()}";
            if (grantType == GrantType.AuthorizationCode)
                body += $"&code={code}";
            else if (grantType == GrantType.RefreshToken)
                body += $"&refresh_token={Uri.EscapeDataString(code)}";

            HttpContent postBody = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _clientKey);

            HttpResponseMessage responseBase = null;

            if (_config.AuthVersion == AuthVersion.v1)
                responseBase = await _client.PostAsync($"{_ssoUrl}/oauth/token", postBody);
            else if (_config.AuthVersion == AuthVersion.v2)
                responseBase = await _client.PostAsync($"{_ssoUrl}/v2/oauth/token", postBody);

            var response = await responseBase.Content.ReadAsStringAsync();
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
