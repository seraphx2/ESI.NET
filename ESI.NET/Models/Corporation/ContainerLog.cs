using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Corporation
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.CorporationLogic.ContainerLogs"/>.
    /// </summary>
    public class ContainerLog
    {
        /// <summary>
        /// What was done to the container, e.g. <c>add</c>, <c>assemble</c>, <c>configure</c>,
        /// <c>lock</c>, <c>move</c>, <c>repackage</c>, <c>set_name</c>, <c>set_password</c>, or
        /// <c>unlock</c>.
        /// </summary>
        [JsonProperty("action")]
        public string Action { get; set; }

        /// <summary>ID of the character who performed the action.</summary>
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        /// <summary>ID of the container.</summary>
        [JsonProperty("container_id")]
        public long ContainerId { get; set; }

        /// <summary>Type ID of the container.</summary>
        [JsonProperty("container_type_id")]
        public long ContainerTypeId { get; set; }

        /// <summary>
        /// Where within its location the item sits: a hangar, a fitting slot (<c>HiSlot0</c>),
        /// cargo, a drone bay, and so on.
        /// </summary>
        [JsonProperty("location_flag")]
        public string LocationFlag { get; set; }

        /// <summary>ID of the station or structure the container is in.</summary>
        [JsonProperty("location_id")]
        public long LocationId { get; set; }

        /// <summary>Timestamp when this log was created.</summary>
        [JsonProperty("logged_at")]
        public DateTime LoggedAt { get; set; }

        /// <summary>
        /// The container's access configuration after a <c>configure</c> action, as a bitmask.
        /// </summary>
        [JsonProperty("new_config_bitmask")]
        public long NewConfigBitmask { get; set; }

        /// <summary>
        /// The container's access configuration before a <c>configure</c> action, as a bitmask.
        /// </summary>
        [JsonProperty("old_config_bitmask")]
        public long OldConfigBitmask { get; set; }

        /// <summary>
        /// Type of password set if action is of type SetPassword or EnterPassword. One of
        /// <c>config</c>, <c>general</c>.
        /// </summary>
        [JsonProperty("password_type")]
        public string PasswordType { get; set; }

        /// <summary>Quantity of the item being acted upon.</summary>
        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        /// <summary>Type ID of the item being acted upon.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }
    }
}
