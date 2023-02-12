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
        internal static string ETag;

        public static async Task<EsiResponse<T>> Execute<T>(HttpClient client, EsiConfig config, RequestSecurity security, HttpMethod httpMethod, string endpoint, Dictionary<string, string> replacements = null, string[] parameters = null, object body = null, string token = null)
        {
            var path = $"{httpMethod}|{endpoint}";

            if (replacements != null)
                foreach (var property in replacements)
                    endpoint = endpoint.Replace($"{{{property.Key}}}", property.Value);

            var url = $"{config.EsiUrl}latest{endpoint}?datasource={config.DataSource.ToEsiValue()}";

            //Attach query string parameters
            if (parameters != null)
                url += $"&{string.Join("&", parameters)}";

            var request = new HttpRequestMessage(httpMethod, url);

            //Attach token to request header if this endpoint requires an authorized character
            if (security == RequestSecurity.Authenticated)
            {
                if (token == null)
                    throw new ArgumentException("The request endpoint requires SSO authentication and a Token has not been provided.");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (ETag != null)
            {
                request.Headers.Add("If-None-Match", $"\"{ETag}\"");
                ETag = null;
            }

            //Serialize post body data
            if (body != null)
                request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            //Output final object
            return new EsiResponse<T>(await client.SendAsync(request).ConfigureAwait(false), path);
        }

        public enum RequestSecurity
        {
            Public,
            Authenticated
        }
    }
}
