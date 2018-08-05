using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dueltank.domain.unit.tests.ServiceTests
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private CategoryService _sut;
        private ICategoryRepository _categoryRepository;

        [SetUp]
        public void SetUp()
        {
            _categoryRepository = Substitute.For<ICategoryRepository>();

            _sut = new CategoryService(_categoryRepository);
        }

        [Test]
        public async Task Should_Invoke_AllCategories_Once()
        {
            // Arrange
            _categoryRepository.AllCategories().Returns(new List<Category>());

            // Act
            await _sut.AllCategories();

            // Assert
            await _categoryRepository.Received(1).AllCategories();

        }
    }
}