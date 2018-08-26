using System.Collections.Generic;
using System.Collections.Immutable;

namespace ESI.NET.Logic
{
    public class BaseLogic
    {
        public ImmutableDictionary<string, string> NoContentMessages = new Dictionary<string, string>()
        {
            //Calendar
            {"PUT|/characters/{character_id}/calendar/{event_id}/", "Event updated"},

            //Contacts
            {"PUT|/characters/{character_id}/contacts/", "Contacts updated"},
            {"DELETE|/characters/{character_id}/contacts/", "Contacts deleted"},

            //Corporations
            {"PUT|/corporations/{corporation_id}/structures/{structure_id}/", "Structure vulnerability window updated"},

            //Fittings
            {"DELETE|/characters/{character_id}/fittings/{fitting_id}/", ""},

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
        }.ToImmutableDictionary();
    }
}