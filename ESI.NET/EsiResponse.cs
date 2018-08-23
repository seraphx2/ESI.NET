using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using static ESI.NET.EsiRequest;

namespace ESI.NET
{
    public class EsiResponse<T>
    {
        public EsiResponse(HttpResponseMessage response, RequestMethod method, string endpoint)
        {
            StatusCode = response.StatusCode;

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                var stringResult = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Created)
                    Data = JsonConvert.DeserializeObject<T>(stringResult);
                else
                    Message = JsonConvert.DeserializeAnonymousType(stringResult, new { error = string.Empty }).error;
            }
        }

        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}