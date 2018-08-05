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
    public class SubCategoryServiceTests
    {
        private SubCategoryService _sut;
        private ISubCategoryRepository _subCategoryRepository;

        [SetUp]
        public void SetUp()
        {
            _subCategoryRepository = Substitute.For<ISubCategoryRepository>();

            _sut = new SubCategoryService(_subCategoryRepository);
        }

        [Test]
        public async Task Should_Invoke_AllSubCategories_Once()
        {
            // Arrange
            _subCategoryRepository.AllSubCategories().Returns(new List<SubCategory>());

            // Act
            await _sut.AllSubCategories();

            // Assert
            await _subCategoryRepository.Received(1).AllSubCategories();

        }
    }
}