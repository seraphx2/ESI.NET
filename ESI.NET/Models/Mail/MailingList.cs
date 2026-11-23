using Newtonsoft.Json;

namespace ESI.NET.Models.Mail
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.MailLogic.MailingLists"/>.</summary>
    public class MailingList
    {
        /// <summary>Mailing list ID.</summary>
        [JsonProperty("mailing_list_id")]
        public long MailingListId { get; set; }

        /// <summary>The mailing list's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
