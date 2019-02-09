using dueltank.application.Queries.DeckSearch;
using dueltank.core.Models.DeckDetails;
using dueltank.core.Models.Search.Decks;
using dueltank.core.Services;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    public class DeckSearchQueryTests
    {
        private IDeckService _deckService;
        private DeckSearchQueryHandler _sut;

        [SetUp]
        public void SetUp()
        {
            _deckService = Substitute.For<IDeckService>();

            _sut = new DeckSearchQueryHandler(_deckService);
        }

        [Test]
        public async Task Given_A_Default_Deck_Search_Criteria_If_User_is_Not_Found_DeckSearch_Should_Be_Empty()
        {
            // Arrange
            var query = new DeckSearchQuery();

            _deckService.Search(Arg.Any<DeckSearchCriteria>()).Returns(new DeckSearchResult { Decks = new List<DeckDetail>()});

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Decks.Should().BeNull();
        }

        [Test]
        public async Task Given_A_Default_Deck_Search_Should_Not_Empty()
        {
            // Arrange
            var query = new DeckSearchQuery();

            _deckService.Search(Arg.Any<DeckSearchCriteria>()).Returns(new DeckSearchResult { Decks = new List<DeckDetail>
            {
                new DeckDetail
                {
                    Id = 234242,
                    Name = "Fire Fist"
                }
            }
            });

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Decks.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Test]
        public async Task Given_A_Default_Deck_Search_Should_Invoke_Search_Once()
        {
            // Arrange
            var query = new DeckSearchQuery();

            _deckService.Search(Arg.Any<DeckSearchCriteria>()).Returns(new DeckSearchResult { Decks = new List<DeckDetail>
            {
                new DeckDetail
                {
                    Id = 234242,
                    Name = "Fire Fist"
                }
            }
            });

            // Act
            await _sut.Handle(query, CancellationToken.None);

            // Assert
            await _deckService.Received(1).Search(Arg.Any<DeckSearchCriteria>());
        }
    }
}