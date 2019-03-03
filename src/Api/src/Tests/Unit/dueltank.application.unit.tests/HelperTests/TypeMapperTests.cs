using dueltank.application.Helpers;
using dueltank.application.Models.Types.Output;
using dueltank.core.Models.Db;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace dueltank.application.unit.tests.HelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class TypeMapperTests
    {
        [Test]
        public void Given_An_Empty_Type_Collection_Should_Map_To_Empty_TypeOutputModel_Collection()
        {
            // Arrange
            var types = new List<Type>();

            // Act
            var result = types.MapToOutputModels();

            // Assert
            result.Should().BeEquivalentTo(new List<TypeOutputModel>());
        }

        [Test]
        public void Given_A_Type_Collection_Should_Map_To_TypeOutputModel_Collection()
        {
            // Arrange
            var expected = new List<TypeOutputModel>
            {
                new TypeOutputModel
                {
                    Id = 1,
                    Name = "SpellCaster"
                }

            };

            var types = new List<Type>
            {
                new Type
                {
                    Id = 1,
                    Name = "SpellCaster"
                }
            };

            // Act
            var result = types.MapToOutputModels();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}