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
    public class SsoLogic
    {
        private readonly HttpClient _client;
        private readonly EsiConfig _config;
        private readonly string clientKey;

        public SsoLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;

            clientKey = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{config.ClientId}:{config.SecretKey}"));
        }

        public string CreateAuthenticationUrl(List<string> scopes = null)
            => $"https://login.eveonline.com/oauth/authorize/?response_type=code&redirect_uri={Uri.EscapeDataString(_config.CallbackUrl)}&client_id={_config.ClientId}{((scopes != null) ? $"&scope={string.Join(" ", scopes)}" : "")}";

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
                body += $"&refresh_token={code}";

            HttpContent postBody = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", clientKey);

            var response = await _client.PostAsync("https://login.eveonline.com/oauth/token", postBody).Result.Content.ReadAsStringAsync();
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
            var response = await _client.GetAsync("https://login.eveonline.com/oauth/verify").Result.Content.ReadAsStringAsync();
            var authorizedCharacter = JsonConvert.DeserializeObject<AuthorizedCharacterData>(response);
            authorizedCharacter.Token = token.AccessToken;
            authorizedCharacter.RefreshToken = token.RefreshToken;

            // Get more specifc details about authorized character to be used in API calls that require this data about the character
            var characterResponse = new CharacterLogic(_client, _config, authorizedCharacter).Affiliation(new int[] { authorizedCharacter.CharacterID }).Result;
            if (characterResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var characterData = characterResponse.Data.First();

                authorizedCharacter.AllianceID = characterData.AllianceId;
                authorizedCharacter.CorporationID = characterData.CorporationId;
                authorizedCharacter.FactionID = characterData.FactionId;
            }

            return authorizedCharacter;
        }
    }
}
