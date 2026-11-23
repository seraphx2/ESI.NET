using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Wallet
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.WalletLogic.CharacterTransactions"/> and
    /// <see cref="ESI.NET.Logic.WalletLogic.CorporationTransactions"/>.
    /// </summary>
    public class Transaction
    {
        /// <summary>Unique transaction ID.</summary>
        [JsonProperty("transaction_id")]
        public long TransactionId { get; set; }

        /// <summary>Date and time of transaction.</summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>Type ID of the item traded.</summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>ID of the station or structure the transaction happened in.</summary>
        [JsonProperty("location_id")]
        public long LocationId { get; set; }

        /// <summary>Amount paid per unit.</summary>
        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }

        /// <summary>How many items changed hands.</summary>
        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        /// <summary>ID of the other party in the transaction.</summary>
        [JsonProperty("client_id")]
        public long ClientId { get; set; }

        /// <summary>
        /// Whether this side of the transaction bought the item (rather than sold it).
        /// </summary>
        [JsonProperty("is_buy")]
        public bool IsBuy { get; set; }

        /// <summary>
        /// Whether the transaction was for the character personally rather than for their
        /// corporation.
        /// </summary>
        [JsonProperty("is_personal")]
        public bool IsPersonal { get; set; }

        /// <summary>-1 if there is no corresponding wallet journal entry.</summary>
        [JsonProperty("journal_ref_id")]
        public long JournalRefId { get; set; }
    }
}
