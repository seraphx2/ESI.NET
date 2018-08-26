using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using static ESI.NET.EsiRequest;

namespace ESI.NET
{
    public class EsiResponse<T>
    {
        public EsiResponse(HttpResponseMessage response, string noContent)
        {
            StatusCode = response.StatusCode;

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                var result = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Created) {

                    if ((result.StartsWith("{") && result.EndsWith("}")) || result.StartsWith("[") && result.EndsWith("]"))
                        Data = JsonConvert.DeserializeObject<T>(result);
                    else
                        Message = result;
                }                        
                else
                    Message = JsonConvert.DeserializeAnonymousType(result, new { error = string.Empty }).error;
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
                Message = noContent;

            if (response.Headers.Contains("X-Pages"))
                Pages = int.Parse(response.Headers.GetValues("X-Pages").First());
        }

        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; } = null;
        public T Data { get; set; }
        public int? Pages { get; set; } = null;
    }
}