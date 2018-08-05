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
    public class TypeServiceTests
    {
        private TypeService _sut;
        private ITypeRepository _typeRepository;

        [SetUp]
        public void SetUp()
        {
            _typeRepository = Substitute.For<ITypeRepository>();

            _sut = new TypeService(_typeRepository);
        }

        [Test]
        public async Task Should_Invoke_AllTypes_Once()
        {
            // Arrange
            _typeRepository.AllTypes().Returns(new List<Type>());

            // Act
            await _sut.AllTypes();

            // Assert
            await _typeRepository.Received(1).AllTypes();

        }
    }
}