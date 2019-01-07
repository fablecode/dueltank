using dueltank.api.Controllers;
using dueltank.application.Models.Attributes.Output;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.tests.core;

namespace dueltank.api.unit.tests.ControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class AttributesControllerTests
    {
        private AttributesController _sut;
        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            _sut = new AttributesController(_mediator);
        }

        [Test]
        public async Task Get_WhenCalled_Should_Return_OkResult()
        {
            // Arrange
            _mediator.Send(Arg.Any<IRequest<IEnumerable<AttributeOutputModel>>>()).Returns(new List<AttributeOutputModel>());

            // Act
            var result = await _sut.Get();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Get_WhenCalled_Should_Return_All_Monster_Attributes()
        {
            // Arrange
            var testData = new List<AttributeOutputModel>
            {
                new AttributeOutputModel
                {
                    Id = 1,
                    Name = "Fire"
                },
                new AttributeOutputModel
                {
                    Id = 2,
                    Name = "Water"
                }
            };

            var expected = testData;

            _mediator.Send(Arg.Any<IRequest<IEnumerable<AttributeOutputModel>>>()).Returns(testData);

            // Act
            var result = await _sut.Get() as OkObjectResult;

            // Assert
            var attributes = result?.Value as IEnumerable<AttributeOutputModel>;
            attributes.Should().BeEquivalentTo(expected);
        }
    }
}