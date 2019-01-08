using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.api.Controllers;
using dueltank.application.Models.Category.Output;
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
    public class CategoriesControllerTests
    {
        private IMediator _mediator;
        private CategoriesController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            _sut = new CategoriesController(_mediator);
        }

        [Test]
        public async Task Get_WhenCalled_Should_Return_OkResult()
        {
            // Arrange
            _mediator.Send(Arg.Any<IRequest<IEnumerable<CategoryOutputModel>>>()).Returns(new List<CategoryOutputModel>());

            // Act
            var result = await _sut.Get();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

        }
    }
}