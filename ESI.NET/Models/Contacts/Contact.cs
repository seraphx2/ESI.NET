using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESI.NET.Models.Contacts
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.ContactsLogic.ListForAlliance"/>,
    /// <see cref="ESI.NET.Logic.ContactsLogic.ListForCharacter"/>, and
    /// <see cref="ESI.NET.Logic.ContactsLogic.ListForCorporation"/>.
    /// </summary>
    public class Contact
    {
        /// <summary>The standing set for the contact, from -10 to 10.</summary>
        [JsonProperty("standing")]
        public decimal Standing { get; set; }

        /// <summary>
        /// What kind of entity the contact is: <c>character</c>, <c>corporation</c>,
        /// <c>alliance</c>, or <c>faction</c>.
        /// </summary>
        [JsonProperty("contact_type")]
        public string ContactType { get; set; }

        /// <summary>
        /// ID of the contact; <see cref="ContactType"/> says what kind of entity it is.
        /// </summary>
        [JsonProperty("contact_id")]
        public long ContactId { get; set; }

        /// <summary>Whether this contact is being watched.</summary>
        [JsonProperty("is_watched")]
        public bool IsWatched { get; set; }

        /// <summary>
        /// Whether this contact is in the blocked list. Note a missing value denotes unknown, not
        /// true or false.
        /// </summary>
        [JsonProperty("is_blocked")]
        public bool IsBlocked { get; set; }

        /// <summary>IDs of the contact labels applied to this contact.</summary>
        [JsonProperty("label_ids")]
        public List<long> LabelIds { get; set; } = new List<long>(); 

    }
}