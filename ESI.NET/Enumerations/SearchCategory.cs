using System;

namespace ESI.NET.Enumerations
{
    [Flags]
    public enum SearchCategory
    {
        agent = 1,
        alliance = 2,
        character = 4,
        constellation = 8,
        corporation = 16,
        faction = 32,
        inventorytype = 64,
        region = 128,
        solarsystem = 256,
        station = 512,
        wormhole = 1024
    }
}
