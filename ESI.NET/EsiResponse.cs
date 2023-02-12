using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace ESI.NET
{
    public class EsiResponse<T>
    {
        public EsiResponse(HttpResponseMessage response, string path)
        {
            try
            {
                StatusCode = response.StatusCode;
                Endpoint = path.Split('|')[1];

                if (response.Headers.Contains("X-ESI-Request-ID"))
                    RequestId = Guid.Parse(response.Headers.GetValues("X-ESI-Request-ID").First());

                if (response.Headers.Contains("X-Pages"))
                    Pages = int.Parse(response.Headers.GetValues("X-Pages").First());

                if (response.Headers.Contains("ETag"))
                    ETag = response.Headers.GetValues("ETag").First().Replace("\"", string.Empty);

                if (response.Content.Headers.Contains("Expires"))
                    Expires = DateTime.Parse(response.Content.Headers.GetValues("Expires").First());

                if (response.Content.Headers.Contains("Last-Modified"))
                    LastModified = DateTime.Parse(response.Content.Headers.GetValues("Last-Modified").First());

                if (response.Headers.Contains("X-Esi-Error-Limit-Remain"))
                    ErrorLimitRemain = int.Parse(response.Headers.GetValues("X-Esi-Error-Limit-Remain").First());

                if (response.Headers.Contains("X-Esi-Error-Limit-Reset"))
                    ErrorLimitReset = int.Parse(response.Headers.GetValues("X-Esi-Error-Limit-Reset").First());

                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode == HttpStatusCode.OK ||
                        response.StatusCode == HttpStatusCode.Created)
                    {
                        if ((result.StartsWith("{") && result.EndsWith("}")) || result.StartsWith("[") && result.EndsWith("]"))
                            Data = JsonConvert.DeserializeObject<T>(result);
                        else
                            Message = result;
                    }
                    else if (response.StatusCode == HttpStatusCode.NotModified)
                        Message = "Not Modified";
                    else
                        Message = JsonConvert.DeserializeAnonymousType(result, new { error = string.Empty }).error;
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                    Message = _noContentMessage[path];

            }
            catch (Exception ex)
            {
                Message = response.Content.ReadAsStringAsync().Result;
                Exception = ex;
            }
            finally
            {
                response.Dispose();
            }
        }

        public Guid RequestId { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Endpoint { get; set; }
        public string Version { get; set; } = "latest";
        public DateTime? Expires { get; set; }
        public DateTime? LastModified { get; set; }
        public string ETag { get; set; }
        public int? ErrorLimitRemain { get; set; }
        public int? ErrorLimitReset { get; set; }
        public int? Pages { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public Exception Exception { get; set; }

        private readonly ImmutableDictionary<string, string> _noContentMessage = new Dictionary<string, string>()
        {
            //Calendar
            {"PUT|/characters/{character_id}/calendar/{event_id}/", "Event updated"},

            //Contacts
            {"PUT|/characters/{character_id}/contacts/", "Contacts updated"},
            {"DELETE|/characters/{character_id}/contacts/", "Contacts deleted"},

            //Corporations
            {"PUT|/corporations/{corporation_id}/structures/{structure_id}/", "Structure vulnerability window updated"},

            //Fittings
            {"DELETE|/characters/{character_id}/fittings/{fitting_id}/", "Fitting deleted"},

            //Fleets
            {"PUT|/fleets/{fleet_id}/", "Fleet updated"},
            {"POST|/fleets/{fleet_id}/members/", "Fleet invitation sent"},
            {"DELETE|/fleets/{fleet_id}/members/{member_id}/", "Fleet member kicked"},
            {"PUT|/fleets/{fleet_id}/members/{member_id}/", "Fleet invitation sent"},
            {"DELETE|/fleets/{fleet_id}/wings/{wing_id}/", "Wing deleted"},
            {"PUT|/fleets/{fleet_id}/wings/{wing_id}/", "Wing renamed"},
            {"DELETE|/fleets/{fleet_id}/squads/{squad_id}/", "Squad deleted"},
            {"PUT|/fleets/{fleet_id}/squads/{squad_id}/", "Squad renamed"},

            //Mail
            {"POST|/characters/{character_id}/mail/", "Mail created"},
            {"POST|/characters/{character_id}/mail/labels/", "Label created"},
            {"DELETE|/characters/{character_id}/mail/labels/{label_id}/", "Label deleted"},
            {"PUT|/characters/{character_id}/mail/{mail_id}/", "Mail updated"},
            {"DELETE|/characters/{character_id}/mail/{mail_id}/", "Mail deleted"},

            //User Interface
            {"POST|/ui/openwindow/marketdetails/", "Open window request received"},
            {"POST|/ui/openwindow/contract/", "Open window request received"},
            {"POST|/ui/openwindow/information/", "Open window request received"},
            {"POST|/ui/autopilot/waypoint/", "Open window request received"},
            {"POST|/ui/openwindow/newmail/", "Open window request received"}
        }.ToImmutableDictionary();
    }
}
