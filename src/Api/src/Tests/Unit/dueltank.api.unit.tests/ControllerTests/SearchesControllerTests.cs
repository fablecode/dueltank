using System.Threading.Tasks;
using dueltank.api.Controllers;
using dueltank.application.Models.Archetypes.Output;
using dueltank.application.Models.CardSearches.Output;
using dueltank.application.Models.Decks.Output;
using dueltank.application.Queries.ArchetypeSearch;
using dueltank.application.Queries.CardSearches;
using dueltank.application.Queries.DeckSearch;
using dueltank.tests.core;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.api.unit.tests.ControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class SearchesControllerTests
    {
        private IMediator _mediator;
        private SearchesController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            _sut = new SearchesController(_mediator);
        }

        [Test]
        public async Task Get_WhenCalled_For_CardSearches_Should_Return_OkResult()
        {
            // Arrange
            var searchCriteria = new CardSearchQuery();

            _mediator.Send(Arg.Any<IRequest<CardSearchResultOutputModel>>()).Returns(new CardSearchResultOutputModel());

            // Act
            var result = await _sut.Get(searchCriteria);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Get_WhenCalled_For_ArchetypeSearches_Should_Return_OkResult()
        {
            // Arrange
            var searchCriteria = new ArchetypeSearchQuery();

            _mediator.Send(Arg.Any<IRequest<ArchetypeSearchResultOutputModel>>()).Returns(new ArchetypeSearchResultOutputModel());

            // Act
            var result = await _sut.Get(searchCriteria);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Get_WhenCalled_For_DeckSearches_Should_Return_OkResult()
        {
            // Arrange
            var searchCriteria = new DeckSearchQuery();

            _mediator.Send(Arg.Any<IRequest<DeckSearchResultOutputModel>>()).Returns(new DeckSearchResultOutputModel());

            // Act
            var result = await _sut.Get(searchCriteria);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

    }
}