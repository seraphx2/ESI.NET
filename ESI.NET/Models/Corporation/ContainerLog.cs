using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Corporation
{
    public class ContainerLog
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        [JsonProperty("container_id")]
        public long ContainerId { get; set; }

        [JsonProperty("container_type_id")]
        public long ContainerTypeId { get; set; }

        [JsonProperty("location_flag")]
        public string LocationFlag { get; set; }

        [JsonProperty("location_id")]
        public long LocationId { get; set; }

        [JsonProperty("logged_at")]
        public DateTime LoggedAt { get; set; }

        [JsonProperty("new_config_bitmask")]
        public long NewConfigBitmask { get; set; }

        [JsonProperty("old_config_bitmask")]
        public long OldConfigBitmask { get; set; }

        [JsonProperty("password_type")]
        public string PasswordType { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("type_id")]
        public long TypeId { get; set; }
    }
}
