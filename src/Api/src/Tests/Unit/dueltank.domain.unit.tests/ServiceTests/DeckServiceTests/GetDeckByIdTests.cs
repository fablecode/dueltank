using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Models.DeckDetails;
using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.domain.unit.tests.ServiceTests.DeckServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class GetDeckByIdTests
    {
        private DeckService _sut;
        private IDeckTypeRepository _deckTypeRepository;
        private ICardRepository _cardRepository;
        private IDeckRepository _deckRepository;

        [SetUp]
        public void SetUp()
        {
            _deckRepository = Substitute.For<IDeckRepository>();
            _cardRepository = Substitute.For<ICardRepository>();
            _deckTypeRepository = Substitute.For<IDeckTypeRepository>();
            var deckCardRepository = Substitute.For<IDeckCardRepository>();

            _sut = new DeckService
            (
                _deckRepository,
                _cardRepository,
                _deckTypeRepository,
                deckCardRepository
            );
        }

        [Test]
        public async Task Given_A_DeckId_Should_Invoke_GetDeckById_Once()
        {
            // Arrange
            const int expected = 1;
            const int deckId = 23423;

            _deckRepository.GetDeckById(Arg.Any<long>()).Returns(new DeckDetail{ Id = 23423 });

            // Act
            await _sut.GetDeckById(deckId);

            // Assert
            await _deckRepository.Received(expected).GetDeckById(deckId);
        }
    }
}