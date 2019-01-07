using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.api.Controllers;
using dueltank.application.Models.Cards.Output;
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
    public class CardsControllerTests
    {
        private IMediator _mediator;
        private CardsController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            _sut = new CardsController(_mediator);
        }

        [Test]
        public async Task Get_WhenCalled_With_A_Name_Should_Return_OkResult()
        {
            // Arrange
            const string cardName = "Call Of The Haunted";

            _mediator.Send(Arg.Any<IRequest<CardOutputModel>>()).Returns(new CardOutputModel());

            // Act
            var result = await _sut.Get(cardName);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }
        [Test]
        public async Task Get_WhenCalled_With_A_Name_If_Card_Is_Not_Found_Should_Return_NotFoundResult()
        {
            // Arrange
            const string cardName = "Call Of The Haunted";

            _mediator.Send(Arg.Any<IRequest<CardOutputModel>>()).Returns((CardOutputModel) null);

            // Act
            var result = await _sut.Get(cardName);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}