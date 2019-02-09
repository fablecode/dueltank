using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Queries.CardSearches;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Search.Cards;
using dueltank.core.Services;
using dueltank.tests.core;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.application.unit.tests.QueryTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class CardSearchQueryTests
    {
        private CardSearchQueryHandler _sut;
        private ICardService _cardService;

        [SetUp]
        public void SetUp()
        {
            _cardService = Substitute.For<ICardService>();

            _sut = new CardSearchQueryHandler(_cardService);
        }

        [Test]
        public async Task Given_A_CardSearch_Query_Should_Return_SearchResults()
        {
            // Arrange
            var query = new CardSearchQuery();

            _cardService.Search(Arg.Any<CardSearchCriteria>()).Returns(new CardSearchResult{Cards = new List<CardSearch>()});

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Given_A_CardSearch_Query_Should_Invoke_Search_Method_Once()
        {
            // Arrange
            var query = new CardSearchQuery();

            _cardService.Search(Arg.Any<CardSearchCriteria>()).Returns(new CardSearchResult{Cards = new List<CardSearch>()});

            // Act
            await _sut.Handle(query, CancellationToken.None);

            // Assert
            await _cardService.Received(1).Search(Arg.Any<CardSearchCriteria>());
        }
    }
}