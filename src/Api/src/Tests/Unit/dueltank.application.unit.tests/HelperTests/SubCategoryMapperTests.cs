using dueltank.application.Helpers;
using dueltank.application.Models.SubCategory.Output;
using dueltank.core.Models.Db;
using dueltank.tests.core;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace dueltank.application.unit.tests.HelperTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class SubCategoryMapperTests
    {
        [Test]
        public void Given_An_Empty_SubCategory_Collection_Should_Map_To_Empty_SubCategoryOutputModel_Collection()
        {
            // Arrange
            var subCategories = new List<SubCategory>();

            // Act
            var result = subCategories.MapToOutputModels();

            // Assert
            result.Should().BeEquivalentTo(new List<SubCategoryOutputModel>());
        }

        [Test]
        public void Given_A_SubCategory_Collection_Should_Map_To_SubCategoryOutputModel_Collection()
        {
            // Arrange
            var subCategoryOutputModels = new List<SubCategoryOutputModel>
            {
                new SubCategoryOutputModel
                {
                    Id = 1,
                    Name = "Forbidden"
                }

            };

            var subCategories = new List<SubCategory>
            {
                new SubCategory
                {
                    Id = 1,
                    Name = "Forbidden"
                }
            };

            // Act
            var result = subCategories.MapToOutputModels();

            // Assert
            result.Should().BeEquivalentTo(subCategoryOutputModels);
        }
    }
}