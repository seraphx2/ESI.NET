using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Mail
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.MailLogic.Retrieve"/>.</summary>
    public class Message
    {
        /// <summary>Mail subject.</summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>From whom the mail was sent.</summary>
        [JsonProperty("from")]
        public long From { get; set; }

        /// <summary>When the mail was sent.</summary>
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        /// <summary>Recipients of the mail.</summary>
        [JsonProperty("recipients")]
        public List<Recipient> Recipients { get; set; } = new List<Recipient>();

        /// <summary>Mail's body.</summary>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>Labels attached to the mail.</summary>
        [JsonProperty("labels")]
        public long[] Labels { get; set; }

        /// <summary>Whether the mail is flagged as read.</summary>
        [JsonProperty("read")]
        public bool Read { get; set; }
    }

    /// <summary>
    /// Part of <see cref="ESI.NET.Models.Mail.Header.Recipients"/> and
    /// <see cref="ESI.NET.Models.Mail.Message.Recipients"/>.
    /// </summary>
    public class Recipient
    {
        /// <summary>
        /// What kind of recipient: <c>character</c>, <c>corporation</c>, <c>alliance</c>, or
        /// <c>mailing_list</c>.
        /// </summary>
        [JsonProperty("recipient_type")]
        public string RecipientType { get; set; }

        /// <summary>
        /// ID of the recipient; <see cref="RecipientType"/> says what kind of entity it is.
        /// </summary>
        [JsonProperty("recipient_id")]
        public long RecipientId { get; set; }
    }
}
