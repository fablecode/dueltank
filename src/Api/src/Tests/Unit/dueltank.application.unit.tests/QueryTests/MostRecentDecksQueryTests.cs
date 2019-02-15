using dueltank.application.Queries.MostRecentArchetypes;
using dueltank.core.Models.Archetypes;
using dueltank.core.Services;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Queries.MostRecentDecks;
using dueltank.core.Models.DeckDetails;
using dueltank.tests.core;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class MostRecentDecksQueryTests
    {
        private IDeckService _deckService;
        private MostRecentDecksHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _deckService = Substitute.For<IDeckService>();

            _sut = new MostRecentDecksHandler(_deckService);
        }

        [Test]
        public async Task Given_A_PageSize_Should_Return_Most_Recent_Decks()
        {
            // Arrange
            var query = new MostRecentDecksQuery();

            _deckService.MostRecentDecks(Arg.Any<int>()).Returns(new MostRecentDecksResult { Decks = new List<DeckDetail>()});

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Decks.Should().BeNull();
        }

        [Test]
        public async Task Given_A_PageSize_Should_Invoke_MostRecentDecks_Once()
        {
            // Arrange
            var query = new MostRecentDecksQuery();

            _deckService.MostRecentDecks(Arg.Any<int>()).Returns(new MostRecentDecksResult
            {
                Decks = new List<DeckDetail>
                {
                    new DeckDetail
                    {
                        Id = 234224,
                        Name = "Random Deck"
                    }
                }
            });

            // Act
            await _sut.Handle(query, CancellationToken.None);

            // Assert
            await _deckService.Received(1).MostRecentDecks(Arg.Any<int>());
        }
    }
}