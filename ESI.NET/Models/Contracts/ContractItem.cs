using Newtonsoft.Json;

namespace ESI.NET.Models.Contracts
{
    /// <summary>
    /// Returned by <see cref="ESI.NET.Logic.ContractsLogic.CharacterContractItems"/>,
    /// <see cref="ESI.NET.Logic.ContractsLogic.ContractItems"/>, and
    /// <see cref="ESI.NET.Logic.ContractsLogic.CorporationContractItems"/>.
    /// </summary>
    public class ContractItem
    {
        /// <summary>Unique ID for the item, used by the contract system.</summary>
        [JsonProperty("record_id")]
        public long RecordId { get; set; }

        /// <summary>
        /// Type ID of the item: what kind of item it is. Names and details are in the SDE's types
        /// data.
        /// </summary>
        [JsonProperty("type_id")]
        public long TypeId { get; set; }

        /// <summary>Number of items in the stack.</summary>
        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        /// <summary>
        /// -1 indicates that the item is a singleton (non-stackable). If the item happens to be a
        /// Blueprint, -1 is an Original and -2 is a Blueprint Copy.
        /// </summary>
        [JsonProperty("raw_quantity")]
        public long RawQuantity { get; set; }

        /// <summary>
        /// Whether the item is assembled. Packaged items stack; assembled ones (ships, containers)
        /// are singletons and can hold other items.
        /// </summary>
        [JsonProperty("is_singleton")]
        public bool IsSingleton { get; set; }

        /// <summary>
        /// True if the contract issuer has submitted this item with the contract, false if the
        /// isser is asking for this item in the contract.
        /// </summary>
        [JsonProperty("is_included")]
        public bool IsIncluded { get; set; }

        /// <summary>
        /// Whether the item is a blueprint copy (BPC) rather than an original. Absent for anything
        /// that is not a blueprint.
        /// </summary>
        [JsonProperty("is_blueprint_copy")]
        public bool IsBlueprintCopy { get; set; }

        /// <summary>
        /// Unique ID for the item being sold. Not present if item is being requested by contract
        /// rather than sold with contract.
        /// </summary>
        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        /// <summary>Material Efficiency Level of the blueprint.</summary>
        [JsonProperty("material_efficiency")]
        public long MaterialEfficiency { get; set; }

        /// <summary>
        /// Number of runs remaining if the blueprint is a copy, -1 if it is an original.
        /// </summary>
        [JsonProperty("runs")]
        public long Runs { get; set; }

        /// <summary>Time Efficiency Level of the blueprint.</summary>
        [JsonProperty("time_efficiency")]
        public long TimeEfficiency { get; set; }
    }
}
