using System.Collections.Generic;
using System.Threading.Tasks;
using ESI.NET.Models.Mail;

namespace ESI.NET.Logic.Interfaces
{
    public interface IMailLogic
    {
        Task<ApiResponse<Message>> Delete(int mail_id);
        Task<ApiResponse<string>> DeleteLabel(long label_id);
        Task<ApiResponse<List<Header>>> Headers(long[] labels = null, int last_mail_id = 0);
        Task<ApiResponse<LabelCounts>> Labels();
        Task<ApiResponse<List<MailingList>>> MailingLists();
        Task<ApiResponse<int>> New(object[] recipients, string subject, string body, int approved_cost = 0);
        Task<ApiResponse<long>> NewLabel(string name, string color);
        Task<ApiResponse<Message>> Retrieve(int mail_id);
        Task<ApiResponse<Message>> Update(int mail_id, bool? is_read = null, int[] labels = null);
    }
}