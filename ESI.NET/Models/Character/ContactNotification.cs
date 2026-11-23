using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Character
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.CharacterLogic.ContactNotifications"/>.
    /// </summary>
    public class ContactNotification
    {
        /// <summary>The notification's message, which may contain EVE client markup.</summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>ID of the notification.</summary>
        [JsonProperty("notification_id")]
        public long NotificationId { get; set; }

        /// <summary>When the notification was sent.</summary>
        [JsonProperty("send_date")]
        public DateTime SendDate { get; set; }

        /// <summary>ID of the character who added this character as a contact.</summary>
        [JsonProperty("sender_character_id")]
        public long SenderCharacterId { get; set; }

        /// <summary>
        /// A number representing the standing level the receiver has been added at by the sender.
        /// The standing levels are as follows: -10 -&gt; Terrible | -5 -&gt; Bad | 0 -&gt; Neutral
        /// | 5 -&gt; Good | 10 -&gt; Excellent.
        /// </summary>
        [JsonProperty("standing_level")]
        public decimal StandingLevel { get; set; }
    }
}
