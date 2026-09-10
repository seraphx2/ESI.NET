using System.Reflection;
using ESI.NET.Logic;
using ESI.NET.Models.Dogma;
using Newtonsoft.Json;
using Xunit;

namespace ESI.NET.Tests
{
    /// <summary>
    /// Deserialization coverage for the Dogma models against the shapes in the current ESI schema
    /// (esi.evetech.net/meta/openapi.json), and guards for the type split:
    /// <c>Attribute</c>/<c>Effect</c> are the id+value pairs on an item; <c>AttributeInfo</c>/
    /// <c>EffectInfo</c> are the full definitions from <c>/dogma/attributes|effects/{id}/</c>.
    /// The DynamicItem endpoint used to be mistyped as <c>EsiResponse&lt;Effect&gt;</c>.
    /// </summary>
    public class DogmaModelTests
    {
        [Fact]
        public void AttributeInfo_maps_the_full_definition()
        {
            // /dogma/attributes/{attribute_id}/
            const string json = @"{
                ""attribute_id"": 128, ""default_value"": 1.0, ""description"": ""Charge size"",
                ""display_name"": ""Charge Size"", ""high_is_good"": false, ""icon_id"": 12,
                ""name"": ""chargeSize"", ""published"": true, ""stackable"": true, ""unit_id"": 0 }";

            var info = JsonConvert.DeserializeObject<AttributeInfo>(json);

            Assert.Equal(128, info.AttributeId);
            Assert.Equal("chargeSize", info.Name);
            Assert.Equal("Charge Size", info.DisplayName);
            Assert.True(info.Published);
            Assert.False(info.HighIsGood);
        }

        [Fact]
        public void EffectInfo_maps_the_full_definition_including_modifiers()
        {
            // /dogma/effects/{effect_id}/
            const string json = @"{
                ""effect_id"": 10, ""name"": ""online"", ""display_name"": ""Online"",
                ""effect_category"": 4, ""published"": true, ""is_offensive"": false,
                ""modifiers"": [ { ""domain"": ""itemID"", ""func"": ""ItemModifier"",
                    ""modified_attribute_id"": 50, ""modifying_attribute_id"": 51, ""operator"": 6 } ] }";

            var info = JsonConvert.DeserializeObject<EffectInfo>(json);

            Assert.Equal(10, info.EffectId);
            Assert.Equal("online", info.Name);
            Assert.Single(info.Modifiers);
            Assert.Equal("ItemModifier", info.Modifiers[0].Func);
            Assert.Equal(6, info.Modifiers[0].Operator);
        }

        [Fact]
        public void DynamicItem_maps_its_id_value_attribute_and_effect_pairs()
        {
            // /dogma/dynamic/items/{type_id}/{item_id}/
            const string json = @"{
                ""created_by"": 1234567,
                ""dogma_attributes"": [ { ""attribute_id"": 9, ""value"": 450.5 },
                                        { ""attribute_id"": 37, ""value"": 1.12 } ],
                ""dogma_effects"": [ { ""effect_id"": 508, ""is_default"": true } ],
                ""mutator_type_id"": 47800,
                ""source_type_id"": 2410 }";

            var item = JsonConvert.DeserializeObject<DynamicItem>(json);

            Assert.Equal(1234567, item.CreatedBy);
            Assert.Equal(47800, item.MutatorTypeId);
            Assert.Equal(2410, item.SourceTypeId);

            Assert.Equal(2, item.DogmaAttributes.Count);
            Assert.Equal(9, item.DogmaAttributes[0].AttributeId);
            Assert.Equal(450.5, item.DogmaAttributes[0].Value);
            Assert.Equal(1.12, item.DogmaAttributes[1].Value, 5);

            var effect = Assert.Single(item.DogmaEffects);
            Assert.Equal(508, effect.EffectId);
            Assert.True(effect.IsDefault);
        }

        [Fact]
        public void UniverseType_uses_the_same_Dogma_Attribute_and_Effect_pairs()
        {
            // /universe/types/{type_id}/ carries the same {attribute_id,value} / {effect_id,is_default}
            // shape as the dynamic-items endpoint, so both bind to ESI.NET.Models.Dogma.Attribute/Effect.
            const string json = @"{
                ""type_id"": 2410, ""name"": ""Heavy Missile Launcher II"", ""group_id"": 510,
                ""description"": ""..."", ""published"": true,
                ""dogma_attributes"": [ { ""attribute_id"": 51, ""value"": 5.0 } ],
                ""dogma_effects"": [ { ""effect_id"": 40, ""is_default"": false } ] }";

            var type = JsonConvert.DeserializeObject<ESI.NET.Models.Universe.Type>(json);

            Assert.Equal(2410, type.TypeId);
            Attribute attr = Assert.Single(type.DogmaAttributes);
            Assert.Equal(51, attr.AttributeId);
            Assert.Equal(5.0, attr.Value);
            Effect eff = Assert.Single(type.DogmaEffects);
            Assert.Equal(40, eff.EffectId);
        }

        [Theory]
        [InlineData(nameof(DogmaLogic.Attribute), typeof(AttributeInfo))]
        [InlineData(nameof(DogmaLogic.Effect), typeof(EffectInfo))]
        [InlineData(nameof(DogmaLogic.DynamicItem), typeof(DynamicItem))]
        public void DogmaLogic_endpoint_returns_the_expected_payload_type(string method, System.Type expected)
        {
            var returnType = typeof(DogmaLogic)
                .GetMethod(method, BindingFlags.Public | BindingFlags.Instance)
                .ReturnType; // Task<EsiResponse<T>>

            var payloadType = returnType.GetGenericArguments()[0].GetGenericArguments()[0];

            Assert.Equal(expected, payloadType);
        }
    }
}
