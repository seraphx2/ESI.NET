using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESI.NET.Models.Mail;

namespace ESI.NET.Interfaces.Logic
{
    public interface IMailLogic
    {
        /// <summary>
        /// /characters/{character_id}/mail/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<Header>>> Headers(long[] labels = null, int last_mail_id = 0,
            string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/mail/
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="approved_cost"></param>
        /// <returns></returns>
        Task<EsiResponse<int>> New(object[] recipients, string subject, string body, int approved_cost = 0,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/mail/labels/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<LabelCounts>> Labels(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/mail/labels/
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        Task<EsiResponse<long>> NewLabel(string name, string color,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/mail/labels/{label_id}/
        /// </summary>
        /// <param name="label_id"></param>
        /// <returns></returns>
        Task<EsiResponse<string>> DeleteLabel(long label_id, CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/mail/lists/
        /// </summary>
        /// <returns></returns>
        Task<EsiResponse<List<MailingList>>> MailingLists(string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/mail/{mail_id}/
        /// </summary>
        /// <param name="mail_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Message>> Retrieve(int mail_id, string eTag = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/mail/{mail_id}/
        /// </summary>
        /// <param name="mail_id"></param>
        /// <param name="is_read"></param>
        /// <param name="labels"></param>
        /// <returns></returns>
        Task<EsiResponse<Message>> Update(int mail_id, bool? is_read = null, int[] labels = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// /characters/{character_id}/mail/{mail_id}/
        /// </summary>
        /// <param name="mail_id"></param>
        /// <returns></returns>
        Task<EsiResponse<Message>> Delete(int mail_id, CancellationToken cancellationToken = default);
    }
}