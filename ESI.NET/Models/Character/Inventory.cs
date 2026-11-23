// Obsolete and undocumented on purpose: removed with the next compatibility-date release.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using Newtonsoft.Json;

namespace ESI.NET.Models.Character
{
    /// <summary>
    /// Part of ESI's removed character statistics endpoint (<c>/characters/{character_id}/stats/</c>).
    /// No ESI.NET method returns it.
    /// </summary>
    [Obsolete("No ESI endpoint returns this: CCP removed the character statistics endpoint. Will be removed with the next compatibility-date release.")]
    public class Inventory
    {
        [JsonProperty("abandon_loot_quantity")]
        public long AbandonLootQuantity { get; set; }

        [JsonProperty("trash_item_quantity")]
        public long TrashItemQuantity { get; set; }
    }
}
