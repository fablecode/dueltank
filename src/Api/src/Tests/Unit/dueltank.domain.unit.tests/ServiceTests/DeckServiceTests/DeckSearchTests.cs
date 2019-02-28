using dueltank.core.Models.Search.Decks;
using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using dueltank.tests.core;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
using dueltank.Domain.SystemIO;

namespace dueltank.domain.unit.tests.ServiceTests.DeckServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class DeckSearchTests
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
            var deckFileSystem = Substitute.For<IDeckFileSystem>();

            _sut = new DeckService
            (
                _deckRepository,
                _cardRepository,
                _deckTypeRepository,
                deckCardRepository,
                deckFileSystem
            );
        }

        [Test]
        public async Task Given_A_DeckSearchCriteria_Should_Invoke_Search_Once()
        {
            // Arrange
            const int expected = 1;

            _deckRepository.Search(Arg.Any<DeckSearchCriteria>()).Returns(new DeckSearchResult());

            // Act
            await _sut.Search(new DeckSearchCriteria());

            // Assert
            await _deckRepository.Received(expected).Search(Arg.Any<DeckSearchCriteria>());
        }
    }
}