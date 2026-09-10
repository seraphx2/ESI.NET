using ESI.NET.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ESI.NET
{
    internal static class EsiRequest
    {
        public static async Task<EsiResponse<T>> Execute<T>(HttpClient client, EsiConfig config, RequestSecurity security, HttpMethod httpMethod, string endpoint, EsiCallOptions options, Dictionary<string, string> replacements = null, string[] parameters = null, object body = null)
        {
            if (options == null)
                options = new EsiCallOptions();

            var path = $"{httpMethod}|{endpoint}";

            if (replacements != null)
                foreach (var property in replacements)
                    endpoint = endpoint.Replace($"{{{property.Key}}}", property.Value);

            var url = $"{config.EsiUrl}latest{endpoint}?datasource={config.DataSource.ToEsiValue()}";

            //Attach query string parameters
            if (parameters != null)
                url += $"&{string.Join("&", parameters)}";

            if (options.Page.HasValue)
                url += $"&page={options.Page.Value}";

            var request = new HttpRequestMessage(httpMethod, url);

            //Attach token to request header if this endpoint requires an authorized character
            if (security == RequestSecurity.Authenticated)
            {
                var token = options.Character?.Token;
                if (string.IsNullOrEmpty(token))
                    throw new ArgumentException("The request endpoint requires SSO authentication; EsiCallOptions.Character (with a valid Token) has not been provided.");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Hand the character (and any per-call refresh callback) to EsiTokenRefreshHandler,
                // which refreshes a near-expired token and swaps this header before the send.
                EsiRequestState.SetCharacter(request, options.Character);
                if (options.OnTokenRefreshed != null)
                    EsiRequestState.SetCallback(request, options.OnTokenRefreshed);
            }

            if (!string.IsNullOrEmpty(options.IfNoneMatch))
                request.Headers.Add("If-None-Match", $"\"{options.IfNoneMatch.Trim('"')}\"");

            //Serialize post body data
            if (body != null)
                request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            //Output final object
            var response = await client.SendAsync(request, options.CancellationToken).ConfigureAwait(false);
            return await EsiResponse<T>.CreateAsync(response, path, options.CancellationToken).ConfigureAwait(false);
        }

        public enum RequestSecurity
        {
            Public,
            Authenticated
        }
    }
}
