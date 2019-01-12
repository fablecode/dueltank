using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using dueltank.api.Controllers;
using dueltank.api.Models;
using dueltank.application.Commands;
using dueltank.tests.core;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.api.unit.tests.ControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class YgoProDecksControllerTests
    {
        private IMediator _mediator;
        private UserManager<ApplicationUser> _userManager;
        private YgoProDecksController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();

            _userManager = Substitute.For<UserManager<ApplicationUser>>
            (
                Substitute.For<IUserStore<ApplicationUser>>(),
                Substitute.For<IOptions<IdentityOptions>>(),
                Substitute.For<IPasswordHasher<ApplicationUser>>(),
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                Substitute.For<ILookupNormalizer>(),
                Substitute.For<IdentityErrorDescriber>(),
                Substitute.For<IServiceProvider>(),
                Substitute.For<ILogger<UserManager<ApplicationUser>>>()
            );


            _sut = new YgoProDecksController(_mediator, _userManager);
        }

        [Test]
        public async Task Post_WhenCalled_With_An_Invalid_YgoPro_Deck_File_Should_Return_BadRequestObjectResult()
        {
            // Arrange
            const string expected = YgoProDecksController.YgoproDeckFileNotSelected;

            // Act
            var result = await _sut.Post(null) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as string;
            errors.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Post_WhenCalled_With_An_Empty_YgoPro_Deck_File_Should_Return_BadRequestObjectResult()
        {
            // Arrange
            const string expected = YgoProDecksController.YgoproDeckFileIsEmpty;

            var formFile = Substitute.For<IFormFile>();
            formFile.Length.Returns(0);

            // Act
            var result = await _sut.Post(formFile) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as string;
            errors.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Post_WhenCalled_With_A_Valid_YgoPro_Deck_File_And_User_Is_Not_Found_Should_Return_BadRequestResult()
        {
            // Arrange

            var formFile = Substitute.For<IFormFile>();
            var content = new byte[0];
            const string fileName = "Amazoness Deck.ydk";

            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms))
                {
                    writer.Write(content);
                    writer.Flush();
                    ms.Position = 0;
                    formFile.OpenReadStream().Returns(ms);
                    formFile.FileName.Returns(fileName);
                    formFile.Length.Returns(ms.Length);
                }
            }

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns((ApplicationUser)null);

            // Act
            var result = await _sut.Post(formFile) as BadRequestResult;

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task Post_WhenCalled_With_A_Valid_YgoPro_Deck_File_If_Uploaded_Successfully_Should_Return_CreatedAtRouteResult()
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms))
                {
                    // Arrange
                    const string fileName = "Amazoness Deck.ydk";

                    var formFile = Substitute.For<IFormFile>();

                    var content = new byte[] {1, 2, 3, 4, 5};

                    writer.Write(content);
                    writer.Flush();
                    ms.Position = 0;

                    formFile.OpenReadStream().Returns(ms);
                    formFile.FileName.Returns(fileName);
                    formFile.Length.Returns(ms.Length);

                    _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns(new ApplicationUser { Id = Guid.NewGuid().ToString() });

                    _mediator.Send(Arg.Any<IRequest<CommandResult>>()).Returns(new CommandResult { IsSuccessful = true, Data = 2342 });

                    // Act
                    var result = await _sut.Post(formFile) as CreatedAtRouteResult;

                    // Assert
                    result.Should().BeOfType<CreatedAtRouteResult>();
                }
            }

        }

        [Test]
        public async Task Post_WhenCalled_With_A_Valid_YgoPro_Deck_File_If_Upload_Fails_Should_Return_BadRequestObjectResult()
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms))
                {
                    // Arrange
                    const string expected = "40 cards minimum.";

                    const string fileName = "Amazoness Deck.ydk";

                    var formFile = Substitute.For<IFormFile>();

                    var content = new byte[] {1, 2, 3, 4, 5};

                    writer.Write(content);
                    writer.Flush();
                    ms.Position = 0;

                    formFile.OpenReadStream().Returns(ms);
                    formFile.FileName.Returns(fileName);
                    formFile.Length.Returns(ms.Length);

                    _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns(new ApplicationUser { Id = Guid.NewGuid().ToString() });

                    _mediator.Send(Arg.Any<IRequest<CommandResult>>()).Returns(new CommandResult { Errors = new List<string> {"40 cards minimum."} });

                    // Act
                    var result = await _sut.Post(formFile) as BadRequestObjectResult;

                    // Assert
                    result.Should().BeOfType<BadRequestObjectResult>();
                    var errors = result?.Value as IEnumerable<string>;
                    errors.Should().ContainSingle(expected);
                }
            }

        }

    }
}