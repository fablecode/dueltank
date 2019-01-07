using System.Threading.Tasks;
using dueltank.api.Controllers;
using dueltank.application.Models.Archetypes.Output;
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
    public class ArchetypesControllerTests
    {
        private IMediator _mediator;
        private ArchetypesController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            _sut = new ArchetypesController(_mediator);
        }

        [Test]
        public async Task Get_MostRecentArchetypes_WhenCalled_Should_Return_OkResult()
        {
            // Arrange
            const int pageSize = 442;

            _mediator.Send(Arg.Any<IRequest<ArchetypeSearchResultOutputModel>>()).Returns(new ArchetypeSearchResultOutputModel());

            // Act
            var result = await _sut.MostRecentArchetypes(pageSize);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Get_WhenCalled_With_An_ArchetypeId_Should_Return_OkResult()
        {
            // Arrange
            const int archetypeId = 43242;

            _mediator.Send(Arg.Any<IRequest<ArchetypeSearchOutputModel>>()).Returns(new ArchetypeSearchOutputModel());

            // Act
            var result = await _sut.Get(archetypeId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Get_WhenCalled_With_An_ArchetypeId_If_Archetype_Is_Not_Found_Should_Return_NotFoundResult()
        {
            // Arrange
            const int archetypeId = 43242;

            _mediator.Send(Arg.Any<IRequest<ArchetypeSearchOutputModel>>()).Returns((ArchetypeSearchOutputModel) null);

            // Act
            var result = await _sut.Get(archetypeId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

    }
}