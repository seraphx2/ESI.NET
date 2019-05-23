using ESI.NET.Enumerations;
using ESI.NET.Logic;
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
        private readonly string clientKey;
        private readonly string oauthEndpoint;

        public SsoLogic(HttpClient client, EsiConfig config)
        {
            _client = client;
            _config = config;

            clientKey = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{config.ClientId}:{config.SecretKey}"));

            oauthEndpoint = (_config.AuthVersion == AuthVersion.v1) ? "https://login.eveonline.com/oauth" : "https://login.eveonline.com/v2/oauth";
        }

        /// <summary>
        /// Generates the authentication URL that can be used to start the SSO workflow. 
        /// </summary>
        /// <param name="scopes"></param>
        /// <param name="state">Required for Oauth 2.0, the state should be stored in a session and checked in the callback. If no state is provided it will be set to 0</param>
        /// <returns></returns>
        public string CreateAuthenticationUrl(List<string> scopes = null, string state = "0")
            => $"{oauthEndpoint}/authorize/?response_type=code&redirect_uri={Uri.EscapeDataString(_config.CallbackUrl)}&client_id={_config.ClientId}{((scopes != null) ? $"&scope={string.Join(" ", scopes)}" : "")}&state={state}";
        


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

            var response = await _client.PostAsync($"{oauthEndpoint}/token", postBody).Result.Content.ReadAsStringAsync();
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
            var response = await _client.GetAsync($"{oauthEndpoint}/verify").Result.Content.ReadAsStringAsync();
            var authorizedCharacter = JsonConvert.DeserializeObject<AuthorizedCharacterData>(response);
            authorizedCharacter.Token = token.AccessToken;
            authorizedCharacter.RefreshToken = token.RefreshToken;

            var url = $"{_config.EsiUrl}v1/characters/affiliation/?datasource={_config.DataSource.ToEsiValue()}";
            var body = new StringContent(JsonConvert.SerializeObject(new int[] { authorizedCharacter.CharacterID }), Encoding.UTF8, "application/json");

            // Get more specifc details about authorized character to be used in API calls that require this data about the character
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            var characterResponse = await client.PostAsync(url, body).ConfigureAwait(false);

            //var characterResponse = new CharacterLogic(_client, _config, authorizedCharacter).Affiliation(new int[] { authorizedCharacter.CharacterID }).ConfigureAwait(false).GetAwaiter().GetResult();
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
