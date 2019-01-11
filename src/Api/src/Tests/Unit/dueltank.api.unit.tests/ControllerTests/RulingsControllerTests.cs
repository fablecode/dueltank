using dueltank.api.Controllers;
using dueltank.application.Models.Rulings.Output;
using dueltank.tests.core;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dueltank.api.unit.tests.ControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class RulingsControllerTests
    {
        private IMediator _mediator;
        private RulingsController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            _sut = new RulingsController(_mediator);
        }

        [Test]
        public async Task Get_When_Called_With_A_CardId_Should_Return_OkResult()
        {
            // Arrange
            const int cardId = 3242;

            _mediator.Send(Arg.Any<IRequest<IEnumerable<RulingSectionOutputModel>>>()).Returns(new List<RulingSectionOutputModel>());

            // Act
            var result = await _sut.Get(cardId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

        }
    }
}