using Newtonsoft.Json;

namespace ESI.NET.Models.Contacts
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.ContactsLogic.LabelsForAlliance"/>,
    /// <see cref="ESI.NET.Logic.ContactsLogic.LabelsForCharacter"/>, and
    /// <see cref="ESI.NET.Logic.ContactsLogic.LabelsForCorporation"/>.
    /// </summary>
    public class Label
    {
        /// <summary>ID of the label.</summary>
        [JsonProperty("label_id")]
        public long LabelId { get; set; }

        /// <summary>The label's name.</summary>
        [JsonProperty("label_name")]
        public string LabelName { get; set; }
    }
}
