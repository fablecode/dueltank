using dueltank.application.Helpers;
using dueltank.application.Models.Limits.Output;
using dueltank.core.Models.Db;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace dueltank.application.unit.tests.HelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class LimitMapperTests
    {
        [Test]
        public void Given_An_Empty_Limit_Collection_Should_Map_To_Empty_LimitOutputModel_Collection()
        {
            // Arrange
            var limits = new List<Limit>();

            // Act
            var result = limits.MapToOutputModels();

            // Assert
            result.Should().BeEquivalentTo(new List<LimitOutputModel>());
        }

        [Test]
        public void Given_A_Limit_Collection_Should_Map_To_LimitOutputModel_Collection()
        {
            // Arrange
            var limitOutputModels = new List<LimitOutputModel>
            {
                new LimitOutputModel
                {
                    Id = 1,
                    Name = "Forbidden"
                }

            };

            var limits = new List<Limit>
            {
                new Limit
                {
                    Id = 1,
                    Name = "Forbidden"
                }
            };

            // Act
            var result = limits.MapToOutputModels();

            // Assert
            result.Should().BeEquivalentTo(limitOutputModels);
        }
    }
}