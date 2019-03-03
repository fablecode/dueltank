using System.Collections.Generic;
using dueltank.application.Helpers;
using dueltank.application.Models.Attributes.Output;
using dueltank.core.Models.Db;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;

namespace dueltank.application.unit.tests.HelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class AttributeMapperTests
    {
        [Test]
        public void Given_An_Empty_Attribute_Collection_Should_Map_To_Empty_AttributeOutputModel_Collection()
        {
            // Arrange
            var attributes = new List<Attribute>();

            // Act
            var result = attributes.MapToOutputModels();

            // Assert
            result.Should().BeEquivalentTo(new List<AttributeOutputModel>());
        }

        [Test]
        public void Given_An_Attribute_Collection_Should_Map_To_AttributeOutputModel_Collection()
        {
            // Arrange
            var attributeOutput = new List<AttributeOutputModel>
            {
                new AttributeOutputModel
                {
                    Id = 2342,
                    Name = "Earth"
                }

            };

            var attributes = new List<Attribute>
            {
                new Attribute
                {
                    Id = 2342,
                    Name = "Earth"
                }
            };

            // Act
            var result = attributes.MapToOutputModels();

            // Assert
            result.Should().BeEquivalentTo(attributeOutput);
        }
    }
}