using ESI.NET.Enumerations;
using Newtonsoft.Json;
using System;

namespace ESI.NET.Models.Contracts
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.ContractsLogic.CharacterContracts"/>,
    /// <see cref="ESI.NET.Logic.ContractsLogic.Contracts"/>, and
    /// <see cref="ESI.NET.Logic.ContractsLogic.CorporationContracts"/>.
    /// </summary>
    public class Contract
    {
        /// <summary>ID of the contract.</summary>
        [JsonProperty("contract_id")]
        public long ContractId { get; set; }

        /// <summary>Character ID for the issuer.</summary>
        [JsonProperty("issuer_id")]
        public long IssuerId { get; set; }

        /// <summary>Character's corporation ID for the issuer.</summary>
        [JsonProperty("issuer_corporation_id")]
        public long IssuerCorporationId { get; set; }

        /// <summary>
        /// ID to whom the contract is assigned, can be alliance, corporation or character ID.
        /// </summary>
        [JsonProperty("assignee_id")]
        public long AssigneeId { get; set; }

        /// <summary>Who will accept the contract.</summary>
        [JsonProperty("acceptor_id")]
        public long AcceptorId { get; set; }

        /// <summary>Start location ID (for Couriers contract).</summary>
        [JsonProperty("start_location_id")]
        public long StartLocationId { get; set; }

        /// <summary>End location ID (for Couriers contract).</summary>
        [JsonProperty("end_location_id")]
        public long EndLocationId { get; set; }

        /// <summary>
        /// The kind of contract: <c>item_exchange</c>, <c>auction</c>, <c>courier</c>, <c>loan</c>,
        /// or <c>unknown</c>.
        /// </summary>
        [JsonProperty("type")]
        public ContractType Type { get; set; }

        /// <summary>
        /// Status of the the contract. One of <c>outstanding</c>, <c>in_progress</c>,
        /// <c>finished_issuer</c>, <c>finished_contractor</c>, <c>finished</c>, <c>cancelled</c>,
        /// <c>rejected</c>, <c>failed</c>, <c>deleted</c>, <c>reversed</c>.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>The contract's title, as written by its issuer.</summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// True if the contract was issued on behalf of the issuer's corporation.
        /// </summary>
        [JsonProperty("for_corporation")]
        public bool ForCorporation { get; set; }

        /// <summary>
        /// To whom the contract is available. One of <c>public</c>, <c>personal</c>,
        /// <c>corporation</c>, <c>alliance</c>.
        /// </summary>
        [JsonProperty("availability")]
        public string Availability { get; set; }

        /// <summary>Сreation date of the contract.</summary>
        [JsonProperty("date_issued")]
        public DateTime DateIssued { get; set; }

        /// <summary>Expiration date of the contract.</summary>
        [JsonProperty("date_expired")]
        public DateTime DateExpired { get; set; }

        /// <summary>Date of confirmation of contract.</summary>
        [JsonProperty("date_accepted")]
        public DateTime DateAccepted { get; set; }

        /// <summary>Number of days to perform the contract.</summary>
        [JsonProperty("days_to_complete")]
        public long DaysToComplete { get; set; }

        /// <summary>Date of completed of contract.</summary>
        [JsonProperty("date_completed")]
        public DateTime DateCompleted { get; set; }

        /// <summary>Price of contract (for ItemsExchange and Auctions).</summary>
        [JsonProperty("price")]
        public decimal Price { get; set; }

        /// <summary>Remuneration for contract (for Couriers only).</summary>
        [JsonProperty("reward")]
        public decimal Reward { get; set; }

        /// <summary>Collateral price (for Couriers only).</summary>
        [JsonProperty("collateral")]
        public decimal Collateral { get; set; }

        /// <summary>Buyout price (for Auctions only).</summary>
        [JsonProperty("buyout")]
        public decimal Buyout { get; set; }

        /// <summary>Volume of items in the contract.</summary>
        [JsonProperty("volume")]
        public decimal Volume { get; set; }
    }
}
