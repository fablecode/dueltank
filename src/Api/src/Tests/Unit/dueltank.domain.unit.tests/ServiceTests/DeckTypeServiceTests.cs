using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.tests.core;

namespace dueltank.domain.unit.tests.ServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class DeckTypeServiceTests
    {
        private DeckTypeService _sut;
        private IDeckTypeRepository _deckTypeRepository;

        [SetUp]
        public void SetUp()
        {
            _deckTypeRepository = Substitute.For<IDeckTypeRepository>();

            _sut = new DeckTypeService(_deckTypeRepository);
        }

        [Test]
        public async Task Should_Invoke_AllDeckTypes_Once()
        {
            // Arrange
            _deckTypeRepository.AllDeckTypes().Returns(new List<DeckType>());

            // Act
            await _sut.AllDeckTypes();

            // Assert
            await _deckTypeRepository.Received(1).AllDeckTypes();

        }
    }
}