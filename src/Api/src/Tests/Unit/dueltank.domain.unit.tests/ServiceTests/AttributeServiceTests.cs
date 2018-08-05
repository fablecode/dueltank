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
    public class AttributeServiceTests
    {
        private AttributeService _sut;
        private IAttributeRepository _attributeRepository;

        [SetUp]
        public void SetUp()
        {
            _attributeRepository = Substitute.For<IAttributeRepository>();

            _sut = new AttributeService(_attributeRepository);
        }

        [Test]
        public async Task Should_Invoke_AllAttributes_Once()
        {
            // Arrange
            _attributeRepository.AllAttributes().Returns(new List<Attribute>());

            // Act
            await _sut.AllAttributes();

            // Assert
            await _attributeRepository.Received(1).AllAttributes();

        }
    }
}