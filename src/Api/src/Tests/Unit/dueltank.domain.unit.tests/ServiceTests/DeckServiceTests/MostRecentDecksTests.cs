using dueltank.core.Models.DeckDetails;
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
    public class MostRecentDecksTests
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
        public async Task Should_Invoke_MostRecentDecks_Once()
        {
            // Arrange
            const int expected = 1;
            const int pageSize = 20;

            _deckRepository.MostRecentDecks(Arg.Any<int>()).Returns(new MostRecentDecksResult());

            // Act
            await _sut.MostRecentDecks(pageSize);

            // Assert
            await _deckRepository.Received(expected).MostRecentDecks(Arg.Any<int>());
        }
    }
}