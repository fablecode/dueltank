using dueltank.application.Helpers;
using dueltank.application.Models.Category.Output;
using dueltank.core.Models.Db;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace dueltank.application.unit.tests.HelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CategoryMapperTests
    {
        [Test]
        public void Given_An_Empty_Category_Collection_Should_Map_To_Empty_CategoryOutputModel_Collection()
        {
            // Arrange
            var categories = new List<Category>();

            // Act
            var result = categories.MapToOutputModels();

            // Assert
            result.Should().BeEquivalentTo(new List<CategoryOutputModel>());
        }

        [Test]
        public void Given_An_Category_Collection_Should_Map_To_CategoryOutputModel_Collection()
        {
            // Arrange
            var categoryOutput = new List<CategoryOutputModel>
            {
                new CategoryOutputModel
                {
                    Id = 1,
                    Name = "Monster"
                }

            };

            var categories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Monster"
                }
            };

            // Act
            var result = categories.MapToOutputModels();

            // Assert
            result.Should().BeEquivalentTo(categoryOutput);
        }
    }
}