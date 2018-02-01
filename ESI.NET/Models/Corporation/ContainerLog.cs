using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Corporation
{
    public class ContainerLog
    {
        [JsonProperty("logged_at")]
        public DateTime LoggedAt { get; set; }

        [JsonProperty("container_id")]
        public long ContainerId { get; set; }

        [JsonProperty("container_type_id")]
        public int ContainerTypeId { get; set; }

        [JsonProperty("character_id")]
        public int CharacterId { get; set; }

        [JsonProperty("location_id")]
        public long LocationId { get; set; }

        [JsonProperty("location_flag")]
        public string LocationFlag { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("password_type")]
        public string PasswordType { get; set; }

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("old_config_bitmask")]
        public int OldConfigBitmask { get; set; }

        [JsonProperty("new_config_bitmask")]
        public int NewConfigBitmask { get; set; }
    }
}
