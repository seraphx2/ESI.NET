using ESI.NET.Enumerations;
using ESI.NET.Logic;
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
    public class SSOLogic
    {
        private HttpClient _client;
        private ESIConfig _config;
        string clientKey;

        public SSOLogic(HttpClient client, ESIConfig config)
        {
            _client = client;
            _config = config;

            clientKey = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{config.ClientId}:{config.SecretKey}"));
        }

        public string CreateAuthenticationUrl(List<string> scopes)
            => $"https://login.eveonline.com/oauth/authorize/?response_type=code&redirect_uri={Uri.EscapeDataString(_config.CallbackUrl)}&client_id={_config.ClientId}&scope={string.Join(" ", scopes)}";

        /// <summary>
        /// SSO Token helper
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="secretKey"></param>
        /// <param name="grantType"></param>
        /// <param name="code">The authorization_code or the refresh_token</param>
        /// <returns></returns>
        public async Task<SSOToken> GetToken(GrantType grantType, string code)
        {
            var body = $"grant_type={grantType.ToEsiValue()}";
            if (grantType == GrantType.AuthorizationCode)
                body += $"&code={code}";
            else if (grantType == GrantType.RefreshToken)
                body += $"&refresh_token={code}";

            HttpContent postBody = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", clientKey);

            var response = await _client.PostAsync("https://login.eveonline.com/oauth/token", postBody).Result.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<SSOToken>(response);
            token.Expires = DateTime.Now.AddSeconds(token.ExpiresIn);

            return token;
        }

        /// <summary>
        /// Retrieves the Character information for the provided Token information. Serialize and store this object for quick retrieval and token refreshing.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="getExtendedData">Uses ESI to retrieve additional character-specific IDs for use with the client (allianceId, corporationId, factionId).</param>
        /// <returns></returns>
        public async Task<AuthorizedCharacterData> Verify(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync("https://login.eveonline.com/oauth/verify").ConfigureAwait(false);
            var authCharacter = JsonConvert.DeserializeObject<AuthorizedCharacterData>(response.Content.ReadAsStringAsync().Result);
            authCharacter.Token = token;

            // Get more specifc details about authorized character to be used in API calls that require this data about the character
            var characterResponse = new CharacterLogic(_client, _config, authCharacter).Affiliation(new int[] { authCharacter.CharacterID }).Result;
            if (characterResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var characterData = characterResponse.Data.First();

                authCharacter.AllianceID = characterData.AllianceId;
                authCharacter.CorporationID = characterData.CorporationId;
                authCharacter.FactionID = characterData.FactionId;
            }

            return authCharacter;
        }
    }
}
