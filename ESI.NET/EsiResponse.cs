using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

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

            if (response.Content.Headers.Contains("Expires"))
                Expires = DateTime.Parse(response.Content.Headers.GetValues("Expires").First());

            if (response.Headers.Contains("X-Esi-Error-Limit-Remain"))
                ErrorLimitRemain = int.Parse(response.Headers.GetValues("X-Esi-Error-Limit-Remain").First());

            if (response.Headers.Contains("X-Esi-Error-Limit-Reset"))
                ErrorLimitReset = int.Parse(response.Headers.GetValues("X-Esi-Error-Limit-Reset").First());
        }

        public HttpStatusCode StatusCode { get; set; }
        public DateTime? Expires { get; set; }
        public int? ErrorLimitRemain { get; set; }
        public int? ErrorLimitReset { get; set; }
        public int? Pages { get; set; } = null;
        public string Message { get; set; } = null;
        public T Data { get; set; }
    }
}