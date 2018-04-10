using ESI.NET.Enumerations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ESI.NET
{
    public static class SSO
    {
        /// <summary>
        /// SSO Token helper
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="secretKey"></param>
        /// <param name="grant_type"></param>
        /// <param name="code">The authorization_code or the refresh_token</param>
        /// <returns></returns>
        public async static Task<SSOToken> GetToken(string clientId, string secretKey, GrantType grant_type, string code)
        {
            string clientKey = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{secretKey}"));

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://login.eveonline.com/");
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {clientKey}");

            var body = $"grant_type={grant_type}";
            if (grant_type == GrantType.authorization_code)
                body += $"&code={code}";
            else if (grant_type == GrantType.refresh_token)
                body += $"&refresh_token={code}";

            HttpContent postBody = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync("https://login.eveonline.com/oauth/token", postBody).Result.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<SSOToken>(response);
            token.Expires = DateTime.Now.AddSeconds(token.ExpiresIn);

            return token;
        }

        /// <summary>
        /// Retrieves the Character information for the provided Token information
        /// </summary>
        /// <param name="token"></param>
        /// <param name="getExtendedData">Retrieve additional character-specific IDs for use with the client (allianceId, corporationId, factionId</param>
        /// <returns></returns>
        public async static Task<AuthorizedCharacter> Verify(string token, bool getExtendedData)
        {
            // Get Character data from Token authorization
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://login.eveonline.com/");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await client.GetAsync("https://login.eveonline.com/oauth/verify").ConfigureAwait(false);
            var authCharacter = JsonConvert.DeserializeObject<AuthorizedCharacter>(response.Content.ReadAsStringAsync().Result);

            // Get more specifc details about authorized character to be used in API calls that require this data about the character
            if (getExtendedData)
            {
                var manager = new ESIClient(DataSource.tranquility, "ESI.NET Psianna Archeia");
                var characterResponse = manager.Character.Affiliation(new int[] { authCharacter.CharacterID }).Result;
            
                if (characterResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var characterData = characterResponse.Data.First();

                    authCharacter.AllianceID = characterData.AllianceId;
                    authCharacter.CorporationID = characterData.CorporationId;
                    authCharacter.FactionID = characterData.FactionId;
                }
            }

            return authCharacter;
        }
    }

    public class SSOToken
    {
        [JsonProperty("access_token")]
        public string Value { get; set; }

        [JsonProperty("token_type")]
        public string Type { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string Refresh { get; set; }

        public DateTime Expires { get; set; }
    }

    public class AuthorizedCharacter
    {
        public int CharacterID { get; set; }
        public string CharacterName { get; set; }
        public string ExpiresOn { get; set; }
        public string Scopes { get; set; }
        public string TokenType { get; set; }
        public string CharacterOwnerHash { get; set; }

        public int AllianceID { get; set; }
        public int CorporationID { get; set; }
        public int FactionID { get; set; }
    }
}
