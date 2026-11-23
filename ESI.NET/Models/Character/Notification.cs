using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Character
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.CharacterLogic.Notifications"/>.</summary>
    public class Notification
    {
        /// <summary>Whether the notification has been read.</summary>
        [JsonProperty("is_read")]
        public bool IsRead { get; set; }

        /// <summary>ID of the notification.</summary>
        [JsonProperty("notification_id")]
        public long NotificationId { get; set; }

        /// <summary>
        /// ID of the sender; <see cref="SenderType"/> says what kind of entity it is.
        /// </summary>
        [JsonProperty("sender_id")]
        public long SenderId { get; set; }

        /// <summary>
        /// What kind of entity sent the notification: <c>character</c>, <c>corporation</c>,
        /// <c>alliance</c>, <c>faction</c>, or <c>other</c>.
        /// </summary>
        [JsonProperty("sender_type")]
        public string SenderType { get; set; }

        /// <summary>
        /// The notification's details as YAML-style text. Which fields appear depends on
        /// <see cref="Type"/>.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>When the notification was sent.</summary>
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The kind of notification, e.g. <c>StructureUnderAttack</c> or <c>WarDeclared</c>. It
        /// decides what <see cref="Text"/> holds.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
