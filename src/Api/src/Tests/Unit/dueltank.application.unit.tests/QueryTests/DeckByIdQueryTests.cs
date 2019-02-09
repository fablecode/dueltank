using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Queries.DeckById;
using dueltank.core.Models.DeckDetails;
using dueltank.core.Services;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    public class DeckByIdQueryTests
    {
        private IDeckService _deckService;
        private DeckByIdQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _deckService = Substitute.For<IDeckService>();

            _sut = new DeckByIdQueryHandler(_deckService);
        }

        [Test]
        public async Task Given_A_DeckId_If_Deck_Is_Not_Found_Should_Return_Null()
        {
            // Arrange
            var query = new DeckByIdQuery{ DeckId = 234242 };
            _deckService.GetDeckById(Arg.Any<long>()).Returns((DeckDetail) null);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task Given_A_DeckId_If_Deck_Is_Found_Should_Return_Deck()
        {
            // Arrange
            const int expected = 234242;

            var query = new DeckByIdQuery{ DeckId = 234242 };
            _deckService.GetDeckById(Arg.Any<long>()).Returns(new DeckDetail
            {
                Id = 234242,
                Name = "Fire Fist"
            });

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Id.Should().Be(expected);
        }

        [Test]
        public async Task Given_A_DeckId_Should_Invoke_GetDeckById_Once()
        {
            // Arrange
            var query = new DeckByIdQuery{ DeckId = 234242 };
            _deckService.GetDeckById(Arg.Any<long>()).Returns(new DeckDetail
            {
                Id = 234242,
                Name = "Fire Fist"
            });

            // Act
           await _sut.Handle(query, CancellationToken.None);

            // Assert
            await _deckService.Received(1).GetDeckById(Arg.Any<long>());
        }
    }
}