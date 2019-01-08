using dueltank.api.Controllers;
using dueltank.application.Enums;
using dueltank.application.Models.Banlists.Output;
using dueltank.tests.core;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace dueltank.api.unit.tests.ControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class BanlistsControllerTests
    {
        private IMediator _mediator;
        private BanlistsController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            _sut = new BanlistsController(_mediator);
        }

        [Test]
        public async Task Get_Latest_WhenCalled_Should_Return_OkResult()
        {
            // Arrange
            var banlistFormat = BanlistFormat.Tcg;

            _mediator.Send(Arg.Any<IRequest<LatestBanlistOutputModel>>()).Returns(new LatestBanlistOutputModel());

            // Act
            var result = await _sut.Latest(banlistFormat);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Get_MostRecentBanlists_WhenCalled_Should_Return_OkResult()
        {
            // Arrange
            var banlistFormat = BanlistFormat.Tcg;

            _mediator.Send(Arg.Any<IRequest<MostRecentBanlistResultOutput>>()).Returns(new MostRecentBanlistResultOutput());

            // Act
            var result = await _sut.MostRecentBanlists();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }


    }
}