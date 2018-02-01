using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models;
using ESI.NET.Models.Character;

namespace ESI.NET.Logic.Interfaces
{
    public interface ICharacterLogic
    {
        Task<ApiResponse<List<Affiliation>>> Affiliation(List<int> character_ids);
        Task<ApiResponse<List<Agent>>> AgentsResearch();
        Task<ApiResponse<List<Blueprint>>> Blueprints(int page = 1);
        Task<ApiResponse<CSPA>> CalculateCSPA(object character_ids);
        Task<ApiResponse<List<ChatChannel>>> ChatChannels();
        Task<ApiResponse<List<ContactNotification>>> ContactNotifications();
        Task<ApiResponse<List<Corporation>>> CorporationHistory(int character_id);
        Task<ApiResponse<Fatigue>> Fatigue();
        Task<ApiResponse<Information>> Information(int character_id);
        Task<ApiResponse<List<Medal>>> Medals();
        Task<ApiResponse<List<Character>>> Names(List<int> character_ids);
        Task<ApiResponse<List<Notification>>> Notifications();
        Task<ApiResponse<Images>> Portrait(int character_id);
        Task<ApiResponse<List<string>>> Roles();
        Task<ApiResponse<List<Standing>>> Standings();
        Task<ApiResponse<List<Stats>>> Stats();
        Task<ApiResponse<List<Title>>> Titles();
    }
}