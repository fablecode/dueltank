using System.Threading.Tasks;
using dueltank.core.Models.DeckDetails;
using dueltank.Domain.Repository;
using dueltank.Domain.Service;
using dueltank.Domain.SystemIO;
using dueltank.tests.core;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.domain.unit.tests.ServiceTests.DeckServiceTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class SaveDeckThumbnailTests
    {
        private DeckService _sut;
        private IDeckTypeRepository _deckTypeRepository;
        private ICardRepository _cardRepository;
        private IDeckRepository _deckRepository;
        private IDeckFileSystem _deckFileSystem;

        [SetUp]
        public void SetUp()
        {
            _deckRepository = Substitute.For<IDeckRepository>();
            _cardRepository = Substitute.For<ICardRepository>();
            _deckTypeRepository = Substitute.For<IDeckTypeRepository>();
            var deckCardRepository = Substitute.For<IDeckCardRepository>();
            _deckFileSystem = Substitute.For<IDeckFileSystem>();

            _sut = new DeckService
            (
                _deckRepository,
                _cardRepository,
                _deckTypeRepository,
                deckCardRepository,
                _deckFileSystem
            );
        }

        [Test]
        public void Given_A_DeckThumbnail_Should_Invoke_SaveDeckThumbnail_Once()
        {
            // Arrange
            const int expected = 1;

            var deckThumbnailModel = new DeckThumbnail { DeckId = 12312321};

            _deckFileSystem.SaveDeckThumbnail(Arg.Any<DeckThumbnail>()).Returns(23423);

            // Act
            _sut.SaveDeckThumbnail(deckThumbnailModel);

            // Assert
            _deckFileSystem.Received(expected).SaveDeckThumbnail(Arg.Any<DeckThumbnail>());
        }

    }


}