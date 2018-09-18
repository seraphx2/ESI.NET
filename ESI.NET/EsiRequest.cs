using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ESI.NET
{
    internal static class EsiRequest
    {
        internal static string eTag = null;

        public async static Task<EsiResponse<T>> Execute<T>(HttpClient client, EsiConfig config, RequestSecurity security, RequestMethod method, string endpoint, Dictionary<string, string> replacements = null, string[] parameters = null, object body = null, string token = null)
        {
            var path = $"{method.ToString()}|{endpoint}";
            var version = EndpointVersion[path];

            if (replacements != null)
                foreach (var property in replacements)
                    endpoint = endpoint.Replace($"{{{property.Key}}}", property.Value);

            var url = $"{config.EsiUrl}{version}{endpoint}?datasource={config.DataSource.ToEsiValue()}";

            //Attach token to request header if this endpoint requires an authorized character
            if (security == RequestSecurity.Authenticated)
            {
                if (token == null)
                    throw new ArgumentException("The request endpoint requires SSO authentication and a Token has not been provided.");
                else
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (eTag != null)
            {
                client.DefaultRequestHeaders.Add("If-None-Match", $"\"{eTag}\"");
                eTag = null;
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
            var obj = new EsiResponse<T>(response, path, version);
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

        private static readonly ImmutableDictionary<string, string> EndpointVersion = new Dictionary<string, string>()
        {
            {"GET|/alliances/", "v1"},
            {"GET|/alliances/{alliance_id}/", "v3"},
            {"GET|/alliances/{alliance_id}/contacts/", "v2"},
            {"GET|/alliances/{alliance_id}/contacts/labels/", "v1"},
            {"GET|/alliances/{alliance_id}/corporations/", "v1"},
            {"GET|/alliances/{alliance_id}/icons/", "v1"},
            {"POST|/characters/affiliation/", "v1"},
            {"GET|/characters/{character_id}/", "v4"},
            {"GET|/characters/{character_id}/agents_research/", "v1"},
            {"GET|/characters/{character_id}/assets/", "v3"},
            {"POST|/characters/{character_id}/assets/locations/", "v2"},
            {"POST|/characters/{character_id}/assets/names/", "v1"},
            {"GET|/characters/{character_id}/attributes/", "v1"},
            {"GET|/characters/{character_id}/blueprints/", "v2"},
            {"GET|/characters/{character_id}/bookmarks/", "v2"},
            {"GET|/characters/{character_id}/bookmarks/folders/", "v2"},
            {"GET|/characters/{character_id}/calendar/", "v1"},
            {"GET|/characters/{character_id}/calendar/{event_id}/", "v3"},
            {"PUT|/characters/{character_id}/calendar/{event_id}/", "v3"},
            {"GET|/characters/{character_id}/calendar/{event_id}/attendees/", "v1"},
            {"GET|/characters/{character_id}/clones/", "v3"},
            {"GET|/characters/{character_id}/contacts/", "v2"},
            {"POST|/characters/{character_id}/contacts/", "v2"},
            {"PUT|/characters/{character_id}/contacts/", "v2"},
            {"DELETE|/characters/{character_id}/contacts/", "v2"},
            {"GET|/characters/{character_id}/contacts/labels/", "v1"},
            {"GET|/characters/{character_id}/contracts/", "v1"},
            {"GET|/characters/{character_id}/contracts/{contract_id}/bids/", "v1"},
            {"GET|/characters/{character_id}/contracts/{contract_id}/items/", "v1"},
            {"GET|/characters/{character_id}/corporationhistory/", "v1"},
            {"POST|/characters/{character_id}/cspa/", "v4"},
            {"GET|/characters/{character_id}/fatigue/", "v1"},
            {"GET|/characters/{character_id}/fittings/", "v1"},
            {"POST|/characters/{character_id}/fittings/", "v1"},
            {"DELETE|/characters/{character_id}/fittings/{fitting_id}/", "v1"},
            {"GET|/characters/{character_id}/fleet/", "v1"},
            {"GET|/characters/{character_id}/fw/stats/", "v1"},
            {"GET|/characters/{character_id}/implants/", "v1"},
            {"GET|/characters/{character_id}/industry/jobs/", "v1"},
            {"GET|/characters/{character_id}/killmails/recent/", "v1"},
            {"GET|/characters/{character_id}/location/", "v1"},
            {"GET|/characters/{character_id}/loyalty/points/", "v1"},
            {"GET|/characters/{character_id}/mail/", "v1"},
            {"POST|/characters/{character_id}/mail/", "v1"},
            {"GET|/characters/{character_id}/mail/labels/", "v3"},
            {"POST|/characters/{character_id}/mail/labels/", "v2"},
            {"DELETE|/characters/{character_id}/mail/labels/{label_id}/", "v1"},
            {"GET|/characters/{character_id}/mail/lists/", "v1"},
            {"GET|/characters/{character_id}/mail/{mail_id}/", "v1"},
            {"PUT|/characters/{character_id}/mail/{mail_id}/", "v1"},
            {"DELETE|/characters/{character_id}/mail/{mail_id}/", "v1"},
            {"GET|/characters/{character_id}/medals/", "v1"},
            {"GET|/characters/{character_id}/mining/", "v1"},
            {"GET|/characters/{character_id}/notifications/", "v3"},
            {"GET|/characters/{character_id}/notifications/contacts/", "v1"},
            {"GET|/characters/{character_id}/online/", "v2"},
            {"GET|/characters/{character_id}/opportunities/", "v1"},
            {"GET|/characters/{character_id}/orders/", "v2"},
            {"GET|/characters/{character_id}/orders/history/", "v1"},
            {"GET|/characters/{character_id}/planets/", "v1"},
            {"GET|/characters/{character_id}/planets/{planet_id}/", "v3"},
            {"GET|/characters/{character_id}/portrait/", "v2"},
            {"GET|/characters/{character_id}/roles/", "v2"},
            {"GET|/characters/{character_id}/search/", "v3"},
            {"GET|/characters/{character_id}/ship/", "v1"},
            {"GET|/characters/{character_id}/skillqueue/", "v2"},
            {"GET|/characters/{character_id}/skills/", "v4"},
            {"GET|/characters/{character_id}/standings/", "v1"},
            {"GET|/characters/{character_id}/stats/", "v2"},
            {"GET|/characters/{character_id}/titles/", "v1"},
            {"GET|/characters/{character_id}/wallet/", "v1"},
            {"GET|/characters/{character_id}/wallet/journal/", "v4"},
            {"GET|/characters/{character_id}/wallet/transactions/", "v1"},
            {"GET|/contracts/public/bids/{contract_id}/", "v1"},
            {"GET|/contracts/public/items/{contract_id}/", "v1"},
            {"GET|/contracts/public/{region_id}/", "v1"},
            {"GET|/corporation/{corporation_id}/mining/extractions/", "v1"},
            {"GET|/corporation/{corporation_id}/mining/observers/", "v1"},
            {"GET|/corporation/{corporation_id}/mining/observers/{observer_id}/", "v1"},
            {"GET|/corporations/npccorps/", "v1"},
            {"GET|/corporations/{corporation_id}/", "v4"},
            {"GET|/corporations/{corporation_id}/alliancehistory/", "v2"},
            {"GET|/corporations/{corporation_id}/assets/", "v3"},
            {"POST|/corporations/{corporation_id}/assets/locations/", "v2"},
            {"POST|/corporations/{corporation_id}/assets/names/", "v1"},
            {"GET|/corporations/{corporation_id}/blueprints/", "v2"},
            {"GET|/corporations/{corporation_id}/bookmarks/", "v1"},
            {"GET|/corporations/{corporation_id}/bookmarks/folders/", "v1"},
            {"GET|/corporations/{corporation_id}/contacts/", "v2"},
            {"GET|/corporations/{corporation_id}/contacts/labels/", "v1"},
            {"GET|/corporations/{corporation_id}/containers/logs/", "v2"},
            {"GET|/corporations/{corporation_id}/contracts/", "v1"},
            {"GET|/corporations/{corporation_id}/contracts/{contract_id}/bids/", "v1"},
            {"GET|/corporations/{corporation_id}/contracts/{contract_id}/items/", "v1"},
            {"GET|/corporations/{corporation_id}/customs_offices/", "v1"},
            {"GET|/corporations/{corporation_id}/divisions/", "v1"},
            {"GET|/corporations/{corporation_id}/facilities/", "v1"},
            {"GET|/corporations/{corporation_id}/fw/stats/", "v1"},
            {"GET|/corporations/{corporation_id}/icons/", "v1"},
            {"GET|/corporations/{corporation_id}/industry/jobs/", "v1"},
            {"GET|/corporations/{corporation_id}/killmails/recent/", "v1"},
            {"GET|/corporations/{corporation_id}/medals/", "v1"},
            {"GET|/corporations/{corporation_id}/medals/issued/", "v1"},
            {"GET|/corporations/{corporation_id}/members/", "v3"},
            {"GET|/corporations/{corporation_id}/members/limit/", "v1"},
            {"GET|/corporations/{corporation_id}/members/titles/", "v1"},
            {"GET|/corporations/{corporation_id}/membertracking/", "v1"},
            {"GET|/corporations/{corporation_id}/orders/", "v3"},
            {"GET|/corporations/{corporation_id}/orders/history/", "v2"},
            {"GET|/corporations/{corporation_id}/roles/", "v1"},
            {"GET|/corporations/{corporation_id}/roles/history/", "v1"},
            {"GET|/corporations/{corporation_id}/shareholders/", "v1"},
            {"GET|/corporations/{corporation_id}/standings/", "v1"},
            {"GET|/corporations/{corporation_id}/starbases/", "v1"},
            {"GET|/corporations/{corporation_id}/starbases/{starbase_id}/", "v1"},
            {"GET|/corporations/{corporation_id}/structures/", "v2"},
            {"GET|/corporations/{corporation_id}/titles/", "v1"},
            {"GET|/corporations/{corporation_id}/wallets/", "v1"},
            {"GET|/corporations/{corporation_id}/wallets/{division}/journal/", "v3"},
            {"GET|/corporations/{corporation_id}/wallets/{division}/transactions/", "v1"},
            {"GET|/dogma/attributes/", "v1"},
            {"GET|/dogma/attributes/{attribute_id}/", "v1"},
            {"GET|/dogma/dynamic/items/{type_id}/{item_id}/", "v1"},
            {"GET|/dogma/effects/", "v1"},
            {"GET|/dogma/effects/{effect_id}/", "v2"},
            {"GET|/fleets/{fleet_id}/", "v1"},
            {"PUT|/fleets/{fleet_id}/", "v1"},
            {"GET|/fleets/{fleet_id}/members/", "v1"},
            {"POST|/fleets/{fleet_id}/members/", "v1"},
            {"PUT|/fleets/{fleet_id}/members/{member_id}/", "v1"},
            {"DELETE|/fleets/{fleet_id}/members/{member_id}/", "v1"},
            {"PUT|/fleets/{fleet_id}/squads/{squad_id}/", "v1"},
            {"DELETE|/fleets/{fleet_id}/squads/{squad_id}/", "v1"},
            {"GET|/fleets/{fleet_id}/wings/", "v1"},
            {"POST|/fleets/{fleet_id}/wings/", "v1"},
            {"PUT|/fleets/{fleet_id}/wings/{wing_id}/", "v1"},
            {"DELETE|/fleets/{fleet_id}/wings/{wing_id}/", "v1"},
            {"POST|/fleets/{fleet_id}/wings/{wing_id}/squads/", "v1"},
            {"GET|/fw/leaderboards/", "v1"},
            {"GET|/fw/leaderboards/characters/", "v1"},
            {"GET|/fw/leaderboards/corporations/", "v1"},
            {"GET|/fw/stats/", "v1"},
            {"GET|/fw/systems/", "v2"},
            {"GET|/fw/wars/", "v1"},
            {"GET|/incursions/", "v1"},
            {"GET|/industry/facilities/", "v1"},
            {"GET|/industry/systems/", "v1"},
            {"GET|/insurance/prices/", "v1"},
            {"GET|/killmails/{killmail_id}/{killmail_hash}/", "v1"},
            {"GET|/loyalty/stores/{corporation_id}/offers/", "v1"},
            {"GET|/markets/groups/", "v1"},
            {"GET|/markets/groups/{market_group_id}/", "v1"},
            {"GET|/markets/prices/", "v1"},
            {"GET|/markets/structures/{structure_id}/", "v1"},
            {"GET|/markets/{region_id}/history/", "v1"},
            {"GET|/markets/{region_id}/orders/", "v1"},
            {"GET|/markets/{region_id}/types/", "v1"},
            {"GET|/opportunities/groups/", "v1"},
            {"GET|/opportunities/groups/{group_id}/", "v1"},
            {"GET|/opportunities/tasks/", "v1"},
            {"GET|/opportunities/tasks/{task_id}/", "v1"},
            {"GET|/route/{origin}/{destination}/", "v1"},
            {"GET|/search/", "v2"},
            {"GET|/sovereignty/campaigns/", "v1"},
            {"GET|/sovereignty/map/", "v1"},
            {"GET|/sovereignty/structures/", "v1"},
            {"GET|/status/", "v1"},
            {"POST|/ui/autopilot/waypoint/", "v2"},
            {"POST|/ui/openwindow/contract/", "v1"},
            {"POST|/ui/openwindow/information/", "v1"},
            {"POST|/ui/openwindow/marketdetails/", "v1"},
            {"POST|/ui/openwindow/newmail/", "v1"},
            {"GET|/universe/ancestries/", "v1"},
            {"GET|/universe/asteroid_belts/{asteroid_belt_id}/", "v1"},
            {"GET|/universe/bloodlines/", "v1"},
            {"GET|/universe/categories/", "v1"},
            {"GET|/universe/categories/{category_id}/", "v1"},
            {"GET|/universe/constellations/", "v1"},
            {"GET|/universe/constellations/{constellation_id}/", "v1"},
            {"GET|/universe/factions/", "v2"},
            {"GET|/universe/graphics/", "v1"},
            {"GET|/universe/graphics/{graphic_id}/", "v1"},
            {"GET|/universe/groups/", "v1"},
            {"GET|/universe/groups/{group_id}/", "v1"},
            {"POST|/universe/ids/", "v1"},
            {"GET|/universe/moons/{moon_id}/", "v1"},
            {"POST|/universe/names/", "v2"},
            {"GET|/universe/planets/{planet_id}/", "v1"},
            {"GET|/universe/races/", "v1"},
            {"GET|/universe/regions/", "v1"},
            {"GET|/universe/regions/{region_id}/", "v1"},
            {"GET|/universe/schematics/{schematic_id}/", "v1"},
            {"GET|/universe/stargates/{stargate_id}/", "v1"},
            {"GET|/universe/stars/{star_id}/", "v1"},
            {"GET|/universe/stations/{station_id}/", "v2"},
            {"GET|/universe/structures/", "v1"},
            {"GET|/universe/structures/{structure_id}/", "v2"},
            {"GET|/universe/system_jumps/", "v1"},
            {"GET|/universe/system_kills/", "v2"},
            {"GET|/universe/systems/", "v1"},
            {"GET|/universe/systems/{system_id}/", "v4"},
            {"GET|/universe/types/", "v1"},
            {"GET|/universe/types/{type_id}/", "v3"},
            {"GET|/wars/", "v1"},
            {"GET|/wars/{war_id}/", "v1"},
            {"GET|/wars/{war_id}/killmails/", "v1" }
        }.ToImmutableDictionary();
    }
}