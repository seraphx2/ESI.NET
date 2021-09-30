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
        internal static string ETag;

        public static async Task<EsiResponse<T>> Execute<T>(HttpClient client, EsiConfig config, RequestSecurity security, RequestMethod method, string endpoint, Dictionary<string, string> replacements = null, string[] parameters = null, object body = null, string token = null)
        {
            client.DefaultRequestHeaders.Clear();

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

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (ETag != null)
            {
                client.DefaultRequestHeaders.Add("If-None-Match", $"\"{ETag}\"");
                ETag = null;
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
                case RequestMethod.Delete:
                    response = await client.DeleteAsync(url).ConfigureAwait(false);
                    break;

                case RequestMethod.Get:
                    response = await client.GetAsync(url).ConfigureAwait(false);
                    break;

                case RequestMethod.Post:
                    response = await client.PostAsync(url, postBody).ConfigureAwait(false);
                    break;

                case RequestMethod.Put:
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
            Delete,
            Get,
            Post,
            Put
        }

        private static readonly ImmutableDictionary<string, string> EndpointVersion = new Dictionary<string, string>()
        {
            {"Get|/alliances/", "v2"},
            {"Get|/alliances/{alliance_id}/", "v4"},
            {"Get|/alliances/{alliance_id}/contacts/", "v2"},
            {"Get|/alliances/{alliance_id}/contacts/labels/", "v1"},
            {"Get|/alliances/{alliance_id}/corporations/", "v2"},
            {"Get|/alliances/{alliance_id}/icons/", "v1"},
            {"Post|/characters/affiliation/", "v2"},
            {"Get|/characters/{character_id}/", "v5"},
            {"Get|/characters/{character_id}/agents_research/", "v2"},
            {"Get|/characters/{character_id}/assets/", "v5"},
            {"Post|/characters/{character_id}/assets/locations/", "v2"},
            {"Post|/characters/{character_id}/assets/names/", "v1"},
            {"Get|/characters/{character_id}/attributes/", "v1"},
            {"Get|/characters/{character_id}/blueprints/", "v3"},
            {"Get|/characters/{character_id}/bookmarks/", "v2"},
            {"Get|/characters/{character_id}/bookmarks/folders/", "v2"},
            {"Get|/characters/{character_id}/calendar/", "v2"},
            {"Get|/characters/{character_id}/calendar/{event_id}/", "v4"},
            {"Put|/characters/{character_id}/calendar/{event_id}/", "v4"},
            {"Get|/characters/{character_id}/calendar/{event_id}/attendees/", "v2"},
            {"Get|/characters/{character_id}/clones/", "v4"},
            {"Get|/characters/{character_id}/contacts/", "v2"},
            {"Post|/characters/{character_id}/contacts/", "v2"},
            {"Put|/characters/{character_id}/contacts/", "v2"},
            {"Delete|/characters/{character_id}/contacts/", "v2"},
            {"Get|/characters/{character_id}/contacts/labels/", "v1"},
            {"Get|/characters/{character_id}/contracts/", "v1"},
            {"Get|/characters/{character_id}/contracts/{contract_id}/bids/", "v1"},
            {"Get|/characters/{character_id}/contracts/{contract_id}/items/", "v1"},
            {"Get|/characters/{character_id}/corporationhistory/", "v2"},
            {"Post|/characters/{character_id}/cspa/", "v5"},
            {"Get|/characters/{character_id}/fatigue/", "v2"},
            {"Get|/characters/{character_id}/fittings/", "v2"},
            {"Post|/characters/{character_id}/fittings/", "v2"},
            {"Delete|/characters/{character_id}/fittings/{fitting_id}/", "v1"},
            {"Get|/characters/{character_id}/fleet/", "v1"},
            {"Get|/characters/{character_id}/fw/stats/", "v2"},
            {"Get|/characters/{character_id}/implants/", "v2"},
            {"Get|/characters/{character_id}/industry/jobs/", "v1"},
            {"Get|/characters/{character_id}/killmails/recent/", "v1"},
            {"Get|/characters/{character_id}/location/", "v2"},
            {"Get|/characters/{character_id}/loyalty/points/", "v1"},
            {"Get|/characters/{character_id}/mail/", "v1"},
            {"Post|/characters/{character_id}/mail/", "v1"},
            {"Get|/characters/{character_id}/mail/labels/", "v3"},
            {"Post|/characters/{character_id}/mail/labels/", "v2"},
            {"Delete|/characters/{character_id}/mail/labels/{label_id}/", "v1"},
            {"Get|/characters/{character_id}/mail/lists/", "v1"},
            {"Get|/characters/{character_id}/mail/{mail_id}/", "v1"},
            {"Put|/characters/{character_id}/mail/{mail_id}/", "v1"},
            {"Delete|/characters/{character_id}/mail/{mail_id}/", "v1"},
            {"Get|/characters/{character_id}/medals/", "v2"},
            {"Get|/characters/{character_id}/mining/", "v1"},
            {"Get|/characters/{character_id}/notifications/", "v6"},
            {"Get|/characters/{character_id}/notifications/contacts/", "v2"},
            {"Get|/characters/{character_id}/online/", "v3"},
            {"Get|/characters/{character_id}/opportunities/", "v1"},
            {"Get|/characters/{character_id}/orders/", "v2"},
            {"Get|/characters/{character_id}/orders/history/", "v1"},
            {"Get|/characters/{character_id}/planets/", "v1"},
            {"Get|/characters/{character_id}/planets/{planet_id}/", "v3"},
            {"Get|/characters/{character_id}/portrait/", "v3"},
            {"Get|/characters/{character_id}/roles/", "v3"},
            {"Get|/characters/{character_id}/search/", "v3"},
            {"Get|/characters/{character_id}/ship/", "v2"},
            {"Get|/characters/{character_id}/skillqueue/", "v2"},
            {"Get|/characters/{character_id}/skills/", "v4"},
            {"Get|/characters/{character_id}/standings/", "v2"},
            {"Get|/characters/{character_id}/titles/", "v2"},
            {"Get|/characters/{character_id}/wallet/", "v1"},
            {"Get|/characters/{character_id}/wallet/journal/", "v6"},
            {"Get|/characters/{character_id}/wallet/transactions/", "v1"},
            {"Get|/contracts/public/bids/{contract_id}/", "v1"},
            {"Get|/contracts/public/items/{contract_id}/", "v1"},
            {"Get|/contracts/public/{region_id}/", "v1"},
            {"Get|/corporation/{corporation_id}/mining/extractions/", "v1"},
            {"Get|/corporation/{corporation_id}/mining/observers/", "v1"},
            {"Get|/corporation/{corporation_id}/mining/observers/{observer_id}/", "v1"},
            {"Get|/corporations/npccorps/", "v2"},
            {"Get|/corporations/{corporation_id}/", "v5"},
            {"Get|/corporations/{corporation_id}/alliancehistory/", "v3"},
            {"Get|/corporations/{corporation_id}/assets/", "v5"},
            {"Post|/corporations/{corporation_id}/assets/locations/", "v2"},
            {"Post|/corporations/{corporation_id}/assets/names/", "v1"},
            {"Get|/corporations/{corporation_id}/blueprints/", "v3"},
            {"Get|/corporations/{corporation_id}/bookmarks/", "v1"},
            {"Get|/corporations/{corporation_id}/bookmarks/folders/", "v1"},
            {"Get|/corporations/{corporation_id}/contacts/", "v2"},
            {"Get|/corporations/{corporation_id}/contacts/labels/", "v1"},
            {"Get|/corporations/{corporation_id}/containers/logs/", "v3"},
            {"Get|/corporations/{corporation_id}/contracts/", "v1"},
            {"Get|/corporations/{corporation_id}/contracts/{contract_id}/bids/", "v1"},
            {"Get|/corporations/{corporation_id}/contracts/{contract_id}/items/", "v1"},
            {"Get|/corporations/{corporation_id}/customs_offices/", "v1"},
            {"Get|/corporations/{corporation_id}/divisions/", "v2"},
            {"Get|/corporations/{corporation_id}/facilities/", "v2"},
            {"Get|/corporations/{corporation_id}/fw/stats/", "v2"},
            {"Get|/corporations/{corporation_id}/icons/", "v2"},
            {"Get|/corporations/{corporation_id}/industry/jobs/", "v1"},
            {"Get|/corporations/{corporation_id}/killmails/recent/", "v1"},
            {"Get|/corporations/{corporation_id}/medals/", "v2"},
            {"Get|/corporations/{corporation_id}/medals/issued/", "v2"},
            {"Get|/corporations/{corporation_id}/members/", "v4"},
            {"Get|/corporations/{corporation_id}/members/limit/", "v2"},
            {"Get|/corporations/{corporation_id}/members/titles/", "v2"},
            {"Get|/corporations/{corporation_id}/membertracking/", "v2"},
            {"Get|/corporations/{corporation_id}/orders/", "v3"},
            {"Get|/corporations/{corporation_id}/orders/history/", "v2"},
            {"Get|/corporations/{corporation_id}/roles/", "v2"},
            {"Get|/corporations/{corporation_id}/roles/history/", "v2"},
            {"Get|/corporations/{corporation_id}/shareholders/", "v1"},
            {"Get|/corporations/{corporation_id}/standings/", "v2"},
            {"Get|/corporations/{corporation_id}/starbases/", "v2"},
            {"Get|/corporations/{corporation_id}/starbases/{starbase_id}/", "v2"},
            {"Get|/corporations/{corporation_id}/structures/", "v2"},
            {"Get|/corporations/{corporation_id}/titles/", "v2"},
            {"Get|/corporations/{corporation_id}/wallets/", "v1"},
            {"Get|/corporations/{corporation_id}/wallets/{division}/journal/", "v4"},
            {"Get|/corporations/{corporation_id}/wallets/{division}/transactions/", "v1"},
            {"Get|/dogma/attributes/", "v1"},
            {"Get|/dogma/attributes/{attribute_id}/", "v1"},
            {"Get|/dogma/dynamic/items/{type_id}/{item_id}/", "v1"},
            {"Get|/dogma/effects/", "v1"},
            {"Get|/dogma/effects/{effect_id}/", "v2"},
            {"Get|/fleets/{fleet_id}/", "v1"},
            {"Put|/fleets/{fleet_id}/", "v1"},
            {"Get|/fleets/{fleet_id}/members/", "v1"},
            {"Post|/fleets/{fleet_id}/members/", "v1"},
            {"Put|/fleets/{fleet_id}/members/{member_id}/", "v1"},
            {"Delete|/fleets/{fleet_id}/members/{member_id}/", "v1"},
            {"Put|/fleets/{fleet_id}/squads/{squad_id}/", "v1"},
            {"Delete|/fleets/{fleet_id}/squads/{squad_id}/", "v1"},
            {"Get|/fleets/{fleet_id}/wings/", "v1"},
            {"Post|/fleets/{fleet_id}/wings/", "v1"},
            {"Put|/fleets/{fleet_id}/wings/{wing_id}/", "v1"},
            {"Delete|/fleets/{fleet_id}/wings/{wing_id}/", "v1"},
            {"Post|/fleets/{fleet_id}/wings/{wing_id}/squads/", "v1"},
            {"Get|/fw/leaderboards/", "v2"},
            {"Get|/fw/leaderboards/characters/", "v2"},
            {"Get|/fw/leaderboards/corporations/", "v2"},
            {"Get|/fw/stats/", "v2"},
            {"Get|/fw/systems/", "v3"},
            {"Get|/fw/wars/", "v2"},
            {"Get|/incursions/", "v1"},
            {"Get|/industry/facilities/", "v1"},
            {"Get|/industry/systems/", "v1"},
            {"Get|/insurance/prices/", "v1"},
            {"Get|/killmails/{killmail_id}/{killmail_hash}/", "v1"},
            {"Get|/loyalty/stores/{corporation_id}/offers/", "v1"},
            {"Get|/markets/groups/", "v1"},
            {"Get|/markets/groups/{market_group_id}/", "v1"},
            {"Get|/markets/prices/", "v1"},
            {"Get|/markets/structures/{structure_id}/", "v1"},
            {"Get|/markets/{region_id}/history/", "v1"},
            {"Get|/markets/{region_id}/orders/", "v1"},
            {"Get|/markets/{region_id}/types/", "v1"},
            {"Get|/opportunities/groups/", "v1"},
            {"Get|/opportunities/groups/{group_id}/", "v1"},
            {"Get|/opportunities/tasks/", "v1"},
            {"Get|/opportunities/tasks/{task_id}/", "v1"},
            {"Get|/route/{origin}/{destination}/", "v1"},
            {"Get|/search/", "v2"},
            {"Get|/sovereignty/campaigns/", "v1"},
            {"Get|/sovereignty/map/", "v1"},
            {"Get|/sovereignty/structures/", "v1"},
            {"Get|/status/", "v2"},
            {"Post|/ui/autopilot/waypoint/", "v2"},
            {"Post|/ui/openwindow/contract/", "v1"},
            {"Post|/ui/openwindow/information/", "v1"},
            {"Post|/ui/openwindow/marketdetails/", "v1"},
            {"Post|/ui/openwindow/newmail/", "v1"},
            {"Get|/universe/ancestries/", "v1"},
            {"Get|/universe/asteroid_belts/{asteroid_belt_id}/", "v1"},
            {"Get|/universe/bloodlines/", "v1"},
            {"Get|/universe/categories/", "v1"},
            {"Get|/universe/categories/{category_id}/", "v1"},
            {"Get|/universe/constellations/", "v1"},
            {"Get|/universe/constellations/{constellation_id}/", "v1"},
            {"Get|/universe/factions/", "v2"},
            {"Get|/universe/graphics/", "v1"},
            {"Get|/universe/graphics/{graphic_id}/", "v1"},
            {"Get|/universe/groups/", "v1"},
            {"Get|/universe/groups/{group_id}/", "v1"},
            {"Post|/universe/ids/", "v1"},
            {"Get|/universe/moons/{moon_id}/", "v1"},
            {"Post|/universe/names/", "v3"},
            {"Get|/universe/planets/{planet_id}/", "v1"},
            {"Get|/universe/races/", "v1"},
            {"Get|/universe/regions/", "v1"},
            {"Get|/universe/regions/{region_id}/", "v1"},
            {"Get|/universe/schematics/{schematic_id}/", "v1"},
            {"Get|/universe/stargates/{stargate_id}/", "v1"},
            {"Get|/universe/stars/{star_id}/", "v1"},
            {"Get|/universe/stations/{station_id}/", "v2"},
            {"Get|/universe/structures/", "v1"},
            {"Get|/universe/structures/{structure_id}/", "v2"},
            {"Get|/universe/system_jumps/", "v1"},
            {"Get|/universe/system_kills/", "v2"},
            {"Get|/universe/systems/", "v1"},
            {"Get|/universe/systems/{system_id}/", "v4"},
            {"Get|/universe/types/", "v1"},
            {"Get|/universe/types/{type_id}/", "v3"},
            {"Get|/wars/", "v1"},
            {"Get|/wars/{war_id}/", "v1"},
            {"Get|/wars/{war_id}/killmails/", "v1" }
        }.ToImmutableDictionary();
    }
}