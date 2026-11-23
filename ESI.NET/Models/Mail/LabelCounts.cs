using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Mail
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.MailLogic.Labels"/>.</summary>
    public class LabelCounts
    {
        /// <summary>Unread mails across all labels.</summary>
        [JsonProperty("total_unread_count")]
        public long TotalUnreadCount { get; set; }

        /// <summary>The character's mail labels.</summary>
        [JsonProperty("labels")]
        public List<Label> Labels { get; set; } = new List<Label>();
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Mail.LabelCounts.Labels"/>.</summary>
    public class Label
    {
        /// <summary>Unread mails with this label.</summary>
        [JsonProperty("unread_count")]
        public long UnreadCount { get; set; }

        /// <summary>ID of the label.</summary>
        [JsonProperty("label_id")]
        public long LabelId { get; set; }

        /// <summary>The label's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The label's colour as a hex string, e.g. <c>#ffffff</c>.</summary>
        [JsonProperty("color")]
        public string Color { get; set; }
    }
}
