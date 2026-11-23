using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Mail
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.MailLogic.Headers"/>.</summary>
    public class Header
    {
        /// <summary>ID of the mail.</summary>
        [JsonProperty("mail_id")]
        public long MailId { get; set; }

        /// <summary>Mail subject.</summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>From whom the mail was sent.</summary>
        [JsonProperty("from")]
        public long From { get; set; }

        /// <summary>When the mail was sent.</summary>
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        /// <summary>IDs of the labels applied to the mail.</summary>
        [JsonProperty("labels")]
        public long[] Labels { get; set; }

        /// <summary>Recipients of the mail.</summary>
        [JsonProperty("recipients")]
        public List<Recipient> Recipients { get; set; } = new List<Recipient>();

        /// <summary>Whether the mail has been read.</summary>
        [JsonProperty("is_read")]
        public bool IsRead { get; set; }
    }
}
