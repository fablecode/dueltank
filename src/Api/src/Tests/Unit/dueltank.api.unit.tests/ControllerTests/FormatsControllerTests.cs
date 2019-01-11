using dueltank.api.Controllers;
using dueltank.application.Models.Formats.Output;
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
    public class FormatsControllerTests
    {
        private IMediator _mediator;
        private FormatsController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            _sut = new FormatsController(_mediator);
        }

        [Test]
        public async Task Get_WhenCalled_Should_Return_OkResult()
        {
            // Arrange
            _mediator.Send(Arg.Any<IRequest<IEnumerable<FormatOutputModel>>>()).Returns(new List<FormatOutputModel>());

            // Act
            var result = await _sut.Get();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

        }
    }
}