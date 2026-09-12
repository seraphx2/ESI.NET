using ESI.NET.Enumerations;
using Xunit;

namespace ESI.NET.Tests
{
    /// <summary>
    /// <see cref="Extensions.ToEsiValue"/> - the ESI wire string for an enum, for the places that
    /// build a query string / header / form body instead of going through JSON serialization. It
    /// resolves through the same <c>StringEnumConverter</c> every ESI.NET enum declares, so it can
    /// never drift from what actually serializing the enum would produce.
    /// </summary>
    public class ExtensionsTests
    {
        [Fact]
        public void Single_value_enum_resolves_its_EnumMember_value()
        {
            Assert.Equal("wing_commander", FleetRole.WingCommander.ToEsiValue());
        }

        [Fact]
        public void DataSource_resolves_its_EnumMember_value()
        {
            // Regression: DataSource declared [EnumMember] values but no StringEnumConverter, so a
            // direct JsonConvert.SerializeObject would have emitted the underlying int (e.g. "1"),
            // not "tranquility". ToEsiValue happened to route around that via reflection; it no
            // longer does, so the converter has to actually be present.
            Assert.Equal("tranquility", DataSource.Tranquility.ToEsiValue());
        }

        [Fact]
        public void GrantType_resolves_its_EnumMember_value()
        {
            Assert.Equal("refresh_token", GrantType.RefreshToken.ToEsiValue());
        }

        [Fact]
        public void Flags_enum_with_one_set_flag_resolves_it_alone()
        {
            Assert.Equal("agent", SearchCategory.Agent.ToEsiValue());
        }

        [Fact]
        public void Flags_enum_joins_multiple_set_flags_with_no_space()
        {
            // ESI's categories query parameter is a plain comma-separated list. Newtonsoft's own
            // flags serialization joins with ", " (a space) - not what ESI expects - so this has to
            // decompose and rejoin itself rather than just serializing the combined value directly.
            var combo = SearchCategory.Agent | SearchCategory.Faction;

            Assert.Equal("agent,faction", combo.ToEsiValue());
        }

        [Fact]
        public void Flags_enum_preserves_declaration_order_regardless_of_how_flags_were_combined()
        {
            var combo = SearchCategory.Structure | SearchCategory.Agent | SearchCategory.Faction;

            Assert.Equal("agent,faction,structure", combo.ToEsiValue());
        }
    }
}
