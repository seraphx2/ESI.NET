using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ESI.NET.Enumerations
{
    public enum DataSource
    {
        [EnumMember(Value="tranquility")] /**/ Tranquility,
        [EnumMember(Value="singularity")] /**/ Singularity
    }
}
