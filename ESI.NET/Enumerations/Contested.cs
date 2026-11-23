using System.Runtime.Serialization;

namespace ESI.NET.Enumerations
{
    /// <summary>How contested a faction warfare system is.</summary>
    public enum Contested
    {
        /// <summary>The attackers have taken the system.</summary>
        [EnumMember(Value = "captured")]    /**/ Captured,
        /// <summary>The attackers are gaining victory points but the system is not yet vulnerable.</summary>
        [EnumMember(Value = "contested")]   /**/ Contested,
        /// <summary>No attacker progress in the system.</summary>
        [EnumMember(Value = "uncontested")] /**/ Uncontested,
        /// <summary>The attackers have reached the threshold; the system can be captured.</summary>
        [EnumMember(Value = "vulnerable")]  /**/ Vulnerable
    }
}
