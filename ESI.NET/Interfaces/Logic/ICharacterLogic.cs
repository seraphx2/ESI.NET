using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models;
using ESI.NET.Models.Character;

namespace ESI.NET.Interfaces.Logic
{
    public interface ICharacterLogic
    {
        /// <summary>
        /// /characters/affiliation/
        /// </summary>
        /// <param name="characterIds">dynamic = long</param>
        /// <returns></returns>
        Task<EsiResponse<List<Affiliation>>> Affiliation(int[] character_ids,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/names/
        /// </summary>
        /// <param name="characterIds"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Character>>> Names(int[] character_ids, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Information>> Information(int character_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/agents_research/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Agent>>> AgentsResearch(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/blueprints/
        /// </summary>
        /// <param name="page">Which page of results to return</param>
        /// <returns></returns>
        Task<EsiResponse<List<Blueprint>>> Blueprints(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/chat_channels/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<ChatChannel>>> ChatChannels(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/corporationhistory/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        Task<EsiResponse<List<CorporationHistory>>> CorporationHistory(int character_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/cspa/
        /// </summary>
        /// <param name="character_ids">The target characters to calculate the charge for</param>
        /// <returns></returns>
        Task<EsiResponse<decimal>> CSPA(object character_ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/fatigue/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<Fatigue>> Fatigue(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/medals/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Medal>>> Medals(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/notifications/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Notification>>> Notifications(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/notifications/contacts/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<ContactNotification>>> ContactNotifications();

        /// <summary>
        /// /characters/{character_id}/portrait/
        /// </summary>
        /// <param name="character_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Images>> Portrait(int character_id);

        /// <summary>
        /// /characters/{character_id}/roles/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<Roles>> Roles();

        /// <summary>
        /// /characters/{character_id}/standings/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Standing>>> Standings();

        /// <summary>
        /// /characters/{character_id}/titles/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Title>>> Titles();
    }
}