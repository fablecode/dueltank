using dueltank.application.Queries.AllCategories;
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
    public class AllCategoriesQueryTests
    {
        private ICategoryService _categoryService;
        private AllCategoriesQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _categoryService = Substitute.For<ICategoryService>();

            _sut = new AllCategoriesQueryHandler(_categoryService);
        }

        [Test]
        public async Task Given_An_AllCategories_Query_Should_Return_All_Categories()
        {
            // Arrange
            const int expected = 2;

            _categoryService.AllCategories().Returns(new List<Category> {new Category(), new Category()});

            // Act
            var result = await _sut.Handle(new AllCategoriesQuery(), CancellationToken.None);

            // Assert
            result.Should().HaveCount(expected);
        }

        [Test]
        public async Task Given_An_AllCategories_Query_Should_Invoke_AllCategories_Method_Once()
        {
            // Arrange
            const int expected = 1;

            _categoryService.AllCategories().Returns(new List<Category> {new Category(), new Category()});

            // Act
            await _sut.Handle(new AllCategoriesQuery(), CancellationToken.None);

            // Assert
            await _categoryService.Received(expected).AllCategories();
        }
    }
}