using System;
using System.Collections.Generic;
using System.Text;

namespace ESI.NET
{
    public static class Dictionaries
    {
        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<string, string> NoContentMessages = new Dictionary<string, string>()
        {
            //Contacts
            {"PUT|/characters/{character_id}/contacts/", "Contacts updated"},
            {"DELETE|/characters/{character_id}/contacts/", "Contacts deleted"},

            //Corporations
            {"PUT|/corporations/{corporation_id}/structures/{structure_id}/", "Structure vulnerability window updated"},

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
            {"POST|/ui/openwindow/newmail/", "Open window request received"},
        };

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<string, string> EndpointVersions = new Dictionary<string, string>()
        {
            {"/characters/{character_id}/assets/", ""},
            {"/characters/{character_id}/assets/locations/", ""},
            {"/characters/{character_id}/assets/names/", ""},
            {"/corporations/{corporation_id}/assets/", ""},
            {"/corporations/{corporation_id}/assets/locations/", ""},
            {"/corporations/{corporation_id}/assets/names/", ""},

            {"/incursions/", ""},

            {"/insurance/prices/", ""},

            {"/characters/{character_id}/killmails/recent/", ""},
            {"/corporations/{corporation_id}/killmails/recent/", ""},
            {"/killmails/{killmail_id}/{killmail_hash}/", ""},

            {"/characters/{character_id}/planets/", ""},
            {"/characters/{character_id}/planets/{planet_id}/", ""},
            {"/corporations/{corporation_id}/customs_offices/", ""},
            {"/universe/schematics/{schematic_id}/", ""},

            {"/characters/{character_id}/search/", ""},
            {"/search/", ""},

            {"/sovereignty/campaigns/", ""},
            {"/sovereignty/map/", ""},
            {"/sovereignty/structures/", ""},

            {"/wars/", ""},
            {"/wars/{war_id}/", ""},
            {"/wars/{war_id}/killmails/", ""}
        };
    }
}
