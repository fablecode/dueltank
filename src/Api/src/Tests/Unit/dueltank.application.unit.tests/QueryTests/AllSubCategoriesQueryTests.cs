using dueltank.application.Queries.AllSubCategories;
using dueltank.core.Models.Db;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class AllSubCategoriesQueryTests
    {
        private ISubCategoryService _subCategoryService;
        private AllSubCategoriesHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _subCategoryService = Substitute.For<ISubCategoryService>();

            _sut = new AllSubCategoriesHandler(_subCategoryService);
        }

        [Test]
        public async Task Given_An_AllSubCategories_Query_Should_Return_All_SubCategories()
        {
            // Arrange
            const int expected = 2;

            _subCategoryService.AllSubCategories().Returns(new List<SubCategory> {new SubCategory(), new SubCategory()});

            // Act
            var result = await _sut.Handle(new AllSubCategoriesQuery(), CancellationToken.None);

            // Assert
            result.Should().HaveCount(expected);
        }

        [Test]
        public async Task Given_An_AllSubCategories_Query_Should_Invoke_AllSubCategories_Method_Once()
        {
            // Arrange
            const int expected = 1;

            _subCategoryService.AllSubCategories().Returns(new List<SubCategory> {new SubCategory(), new SubCategory()});

            // Act
            await _sut.Handle(new AllSubCategoriesQuery(), CancellationToken.None);

            // Assert
            await _subCategoryService.Received(expected).AllSubCategories();
        }
    }
}