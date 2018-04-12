using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Wallet
{
    public class JournalEntry
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("ref_id")]
        public long RefId { get; set; }

        [JsonProperty("ref_type")]
        public string RefType { get; set; }

        [JsonProperty("first_party_id")]
        public int FirstPartyId { get; set; }

        [JsonProperty("first_party_type")]
        public string FirstPartyType { get; set; }

        [JsonProperty("second_party_id")]
        public int SecondPartyId { get; set; }

        [JsonProperty("second_party_type")]
        public string SecondPartyType { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("tax_receiver_id")]
        public int TaxReceiverId { get; set; }

        [JsonProperty("tax")]
        public decimal Tax { get; set; }

        [JsonProperty("extra_info")]
        public ExtraInfo ExtraInfo { get; set; }
    }

    public class ExtraInfo
    {
        [JsonProperty("location_id")]
        public long LocationId { get; set; }

        [JsonProperty("transaction_id")]
        public long TransactionId { get; set; }

        [JsonProperty("npc_name")]
        public string NpcName { get; set; }

        [JsonProperty("npc_id")]
        public int NpcId { get; set; }

        [JsonProperty("destroyed_ship_type_id")]
        public int DestroyedShipTypeId { get; set; }

        [JsonProperty("character_id")]
        public int CharacterId { get; set; }

        [JsonProperty("corporation_id")]
        public int CorporationId { get; set; }

        [JsonProperty("alliance_id")]
        public int AllianceId { get; set; }

        [JsonProperty("job_id")]
        public int JobId { get; set; }

        [JsonProperty("contract_id")]
        public int ContractId { get; set; }

        [JsonProperty("system_id")]
        public int SystemId { get; set; }

        [JsonProperty("planet_id")]
        public int PlanetId { get; set; }
    }
}
