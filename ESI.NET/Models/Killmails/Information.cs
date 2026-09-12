using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Killmails
{
    public class Information
    {
        [JsonProperty("attackers")]
        public List<Attacker> Attackers { get; set; } = new List<Attacker>();

        [JsonProperty("killmail_id")]
        public long KillmailId { get; set; }

        [JsonProperty("killmail_time")]
        public DateTime KillmailTime { get; set; }

        [JsonProperty("moon_id")]
        public long MoonId { get; set; }

        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        [JsonProperty("victim")]
        public Victim Victim { get; set; }

        [JsonProperty("war_id")]
        public long WarId { get; set; }
    }

    public class Attacker
    {
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        [JsonProperty("damage_done")]
        public long DamageDone { get; set; }

        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        [JsonProperty("final_blow")]
        public bool FinalBlow { get; set; }

        [JsonProperty("security_status")]
        public double SecurityStatus { get; set; }

        [JsonProperty("ship_type_id")]
        public long ShipTypeId { get; set; }

        [JsonProperty("weapon_type_id")]
        public long WeaponTypeId { get; set; }
    }

    public class Victim
    {
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        [JsonProperty("damage_taken")]
        public long DamageTaken { get; set; }

        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        [JsonProperty("ship_type_id")]
        public long ShipTypeId { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; } = new List<Item>();

        [JsonProperty("position")]
        public Position Position { get; set; }
    }

    public class Item
    {
        [JsonProperty("flag")]
        public long Flag { get; set; }

        [JsonProperty("item_type_id")]
        public long ItemTypeId { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("quantity_destroyed")]
        public long QuantityDestroyed { get; set; }

        [JsonProperty("quantity_dropped")]
        public long QuantityDropped { get; set; }

        [JsonProperty("singleton")]
        public long Singleton { get; set; }
    }
}
