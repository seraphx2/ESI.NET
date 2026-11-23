using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ESI.NET.Models.Killmails
{
    /// <summary>Returned by <see cref="ESI.NET.Logic.KillmailsLogic.Information"/>.</summary>
    public class Information
    {
        /// <summary>Everyone who damaged the victim.</summary>
        [JsonProperty("attackers")]
        public List<Attacker> Attackers { get; set; } = new List<Attacker>();

        /// <summary>ID of the killmail.</summary>
        [JsonProperty("killmail_id")]
        public long KillmailId { get; set; }

        /// <summary>Time that the victim was killed and the killmail generated.</summary>
        [JsonProperty("killmail_time")]
        public DateTime KillmailTime { get; set; }

        /// <summary>Moon if the kill took place at one.</summary>
        [JsonProperty("moon_id")]
        public long MoonId { get; set; }

        /// <summary>Solar system that the kill took place in.</summary>
        [JsonProperty("solar_system_id")]
        public long SolarSystemId { get; set; }

        /// <summary>Who and what was destroyed.</summary>
        [JsonProperty("victim")]
        public Victim Victim { get; set; }

        /// <summary>War if the killmail is generated in relation to an official war.</summary>
        [JsonProperty("war_id")]
        public long WarId { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Killmails.Information.Attackers"/>.</summary>
    public class Attacker
    {
        /// <summary>ID of the attacker's alliance, if any.</summary>
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        /// <summary>ID of the attacking character. Absent for NPCs and structures.</summary>
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        /// <summary>ID of the attacker's corporation.</summary>
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        /// <summary>Damage this attacker dealt to the victim.</summary>
        [JsonProperty("damage_done")]
        public long DamageDone { get; set; }

        /// <summary>ID of the attacker's faction (faction warfare or NPC), if any.</summary>
        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        /// <summary>Was the attacker the one to achieve the final blow.</summary>
        [JsonProperty("final_blow")]
        public bool FinalBlow { get; set; }

        /// <summary>Security status for the attacker.</summary>
        [JsonProperty("security_status")]
        public double SecurityStatus { get; set; }

        /// <summary>What ship was the attacker flying.</summary>
        [JsonProperty("ship_type_id")]
        public long ShipTypeId { get; set; }

        /// <summary>What weapon was used by the attacker for the kill.</summary>
        [JsonProperty("weapon_type_id")]
        public long WeaponTypeId { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Killmails.Information.Victim"/>.</summary>
    public class Victim
    {
        /// <summary>ID of the victim's alliance, if any.</summary>
        [JsonProperty("alliance_id")]
        public long AllianceId { get; set; }

        /// <summary>ID of the victim. Absent when the victim was a structure or NPC.</summary>
        [JsonProperty("character_id")]
        public long CharacterId { get; set; }

        /// <summary>ID of the victim's corporation.</summary>
        [JsonProperty("corporation_id")]
        public long CorporationId { get; set; }

        /// <summary>How much total damage was taken by the victim.</summary>
        [JsonProperty("damage_taken")]
        public long DamageTaken { get; set; }

        /// <summary>ID of the victim's faction, if any.</summary>
        [JsonProperty("faction_id")]
        public long FactionId { get; set; }

        /// <summary>The ship that the victim was piloting and was destroyed.</summary>
        [JsonProperty("ship_type_id")]
        public long ShipTypeId { get; set; }

        /// <summary>
        /// Items fitted to or carried in the ship, with how many dropped and were destroyed.
        /// </summary>
        [JsonProperty("items")]
        public List<Item> Items { get; set; } = new List<Item>();

        /// <summary>Coordinates of the victim in Cartesian space relative to the Sun.</summary>
        [JsonProperty("position")]
        public Position Position { get; set; }
    }

    /// <summary>Part of <see cref="ESI.NET.Models.Killmails.Victim.Items"/>.</summary>
    public class Item
    {
        /// <summary>Flag for the location of the item.</summary>
        [JsonProperty("flag")]
        public long Flag { get; set; }

        /// <summary>Type ID of the item.</summary>
        [JsonProperty("item_type_id")]
        public long ItemTypeId { get; set; }

        /// <summary>Items inside this one, e.g. the contents of a container.</summary>
        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        /// <summary>How many of the item were destroyed if any.</summary>
        [JsonProperty("quantity_destroyed")]
        public long QuantityDestroyed { get; set; }

        /// <summary>How many of the item were dropped if any.</summary>
        [JsonProperty("quantity_dropped")]
        public long QuantityDropped { get; set; }

        /// <summary><c>0</c> packaged, <c>1</c> assembled, <c>2</c> a blueprint copy.</summary>
        [JsonProperty("singleton")]
        public long Singleton { get; set; }
    }
}
