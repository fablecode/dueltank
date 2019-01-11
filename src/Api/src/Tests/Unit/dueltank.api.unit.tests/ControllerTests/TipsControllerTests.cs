using dueltank.api.Controllers;
using dueltank.application.Models.SubCategory.Output;
using dueltank.tests.core;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.application.Models.Tips.Output;

namespace dueltank.api.unit.tests.ControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class TipsControllerTests
    {
        private IMediator _mediator;
        private TipsController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            _sut = new TipsController(_mediator);
        }

        [Test]
        public async Task Get_When_Called_With_A_CardId_Should_Return_OkResult()
        {
            // Arrange
            const int cardId = 3242;

            _mediator.Send(Arg.Any<IRequest<IEnumerable<TipSectionOutputModel>>>()).Returns(new List<TipSectionOutputModel>());

            // Act
            var result = await _sut.Get(cardId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

        }
    }
}