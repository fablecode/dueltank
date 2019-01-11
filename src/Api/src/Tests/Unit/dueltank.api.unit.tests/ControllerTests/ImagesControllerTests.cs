using System.Threading.Tasks;
using dueltank.api.Controllers;
using dueltank.application.Queries.ArchetypeImageById;
using dueltank.application.Queries.CardImageByName;
using dueltank.application.Queries.DeckThumbnailImagePath;
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
    public class ImagesControllerTests
    {
        private IMediator _mediator;
        private ImagesController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            _sut = new ImagesController(_mediator);
        }

        [Test]
        public async Task Get_Cards_WhenCalled_With_A_CardName_Should_Return_FileContentResult()
        {
            // Arrange
            const string cardName = "Blue-Eyes White Dragon";

            _mediator.Send(Arg.Any<IRequest<CardImageByNameResult>>()).Returns(new CardImageByNameResult{ ContentType = "image/jpeg", Image = new byte[] {1, 2, 3, 4, 5}});

            // Act
            var result = await _sut.Cards(cardName);

            // Assert
            result.Should().BeOfType<FileContentResult>();
        }

        [Test]
        public async Task Get_DeckThumbnail_WhenCalled_With_A_DeckId_Should_Return_FileContentResult()
        {
            // Arrange
            const int deckId = 980;

            _mediator.Send(Arg.Any<IRequest<DeckThumbnailImagePathQueryResult>>()).Returns(new DeckThumbnailImagePathQueryResult { ContentType = "image/jpeg", Image = new byte[] { 1, 2, 3, 4, 5 } });

            // Act
            var result = await _sut.DeckThumbnail(deckId);

            // Assert
            result.Should().BeOfType<FileContentResult>();
        }

        [Test]
        public async Task Get_Archetypes_WhenCalled_With_A_ArchetypeId_Should_Return_FileContentResult()
        {
            // Arrange
            const int archetypeId = 322;

            _mediator.Send(Arg.Any<IRequest<ArchetypeImageByIdQueryResult>>()).Returns(new ArchetypeImageByIdQueryResult { ContentType = "image/jpeg", Image = new byte[] { 1, 2, 3, 4, 5 } });

            // Act
            var result = await _sut.Archetypes(archetypeId);

            // Assert
            result.Should().BeOfType<FileContentResult>();
        }

    }
}