using ESI.NET.Models.SSO;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ESI.NET
{
    public static class EsiRequest
    {
        public async static Task<EsiResponse<T>> Execute<T>(HttpClient client, EsiConfig config, RequestSecurity security, RequestMethod method, string endpoint, string noContent = null, string[] parameters = null, object body = null, string token = null)
        {
            string version = "latest";
            var url = $"{config.EsiUrl}{version}{endpoint}?datasource={ config.DataSource.ToEsiValue() }";

            //Attach token to request header if this endpoint requires an authorized character
            if (security == RequestSecurity.Authenticated)
            {
                if (token == null)
                    throw new ArgumentException("The request endpoint requires SSO authentication and a Token has not been provided.");
                else
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            //Attach query string parameters
            if (parameters != null)
                url += $"&{string.Join("&", parameters)}";

            //Serialize post body data
            HttpContent postBody = null;
            if (body != null)
                postBody = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            //Get response from client based on request type
            //This is also where body variables will be created and attached as necessary
            HttpResponseMessage response = null;
            switch (method)
            {
                case RequestMethod.DELETE:
                    response = await client.DeleteAsync(url).ConfigureAwait(false);
                    break;

                case RequestMethod.GET:
                    response = await client.GetAsync(url).ConfigureAwait(false);
                    break;

                case RequestMethod.POST:
                    response = await client.PostAsync(url, postBody).ConfigureAwait(false);
                    break;

                case RequestMethod.PUT:
                    response = await client.PutAsync(url, postBody).ConfigureAwait(false);
                    break;
            }
            
            //Output final object
            var obj = new EsiResponse<T>(response, method, endpoint, noContent);
            return obj;
        }

        public enum RequestSecurity
        {
            Public,
            Authenticated
        }

        public enum RequestMethod
        {
            CONNECT,
            DELETE,
            GET,
            HEAD,
            POST,
            PUT,
            TRACE
        }
    }
}