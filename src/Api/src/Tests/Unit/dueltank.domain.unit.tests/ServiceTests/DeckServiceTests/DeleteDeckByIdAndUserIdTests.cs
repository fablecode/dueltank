using System;
using dueltank.core.Models.Search.Decks;
using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using dueltank.tests.core;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace dueltank.domain.unit.tests.ServiceTests.DeckServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class DeleteDeckByIdAndUserIdTests
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
        public async Task Given_A_UserId_Adn_DeckId_Should_Invoke_DeleteDeckByIdAndUserId_Once()
        {
            // Arrange
            const int expected = 1;

            _deckRepository.Search(Arg.Any<DeckSearchCriteria>()).Returns(new DeckSearchResult());

            // Act
            var userId = Guid.NewGuid().ToString();
            const int deckId = 234242;

            await _sut.DeleteDeckByIdAndUserId(userId, deckId);

            // Assert
            await _deckRepository.Received(expected).DeleteDeckByIdAndUserId(userId, deckId);
        }
    }
}