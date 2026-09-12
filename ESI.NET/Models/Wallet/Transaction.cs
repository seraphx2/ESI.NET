using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Wallet
{
    public class Transaction
    {
        [JsonProperty("transaction_id")]
        public long TransactionId { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        [JsonProperty("location_id")]
        public long LocationId { get; set; }

        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("client_id")]
        public long ClientId { get; set; }

        [JsonProperty("is_buy")]
        public bool IsBuy { get; set; }

        [JsonProperty("is_personal")]
        public bool IsPersonal { get; set; }

        [JsonProperty("journal_ref_id")]
        public long JournalRefId { get; set; }
    }
}
