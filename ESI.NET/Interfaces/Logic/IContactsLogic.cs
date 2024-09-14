using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Contacts;

namespace ESI.NET.Interfaces.Logic
{
    public interface IContactsLogic
    {
        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Contact>>> ListForCharacter(int page = 1,
            string eTag = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/contacts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Contact>>> ListForCorporation(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /alliances/{alliance_id}/contacts/
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<EsiResponse<List<Contact>>> ListForAlliance(int page = 1, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="contact_ids"></param>
        /// <param name="standing"></param>
        /// <param name="label_ids"></param>
        /// <param name="watched"></param>
        /// <returns></returns>
        Task<EsiResponse<int[]>> Add(int[] contact_ids, decimal standing, int[] label_ids = null,
            bool? watched = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="contact_id"></param>
        /// <param name="standing"></param>
        /// <param name="label_id"></param>
        /// <param name="watched"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> Update(int[] contact_ids, decimal standing, int[] label_ids = null,
            bool? watched = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/contacts/
        /// </summary>
        /// <param name="contact_ids"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> Delete(int[] contact_ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/contacts/labels/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Label>>> LabelsForCharacter(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /corporations/{corporation_id}/contacts/labels/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Label>>> LabelsForCorporation(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /alliances/{alliance_id}/contacts/labels/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Label>>> LabelsForAlliance();
    }
}