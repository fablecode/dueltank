using dueltank.api.Controllers;
using dueltank.api.Models;
using dueltank.api.Models.Decks.Input;
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dueltank.api.unit.tests.ControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class DeckThumbnailsControllerTests
    {
        private UserManager<ApplicationUser> _userManager;
        private IMediator _mediator;
        private DeckThumbnailsController _sut;

        [SetUp]
        public void SetUp()
        {
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


            _mediator = Substitute.For<IMediator>();

            _sut = new DeckThumbnailsController(_userManager, _mediator);

        }

        [Test]
        public async Task Patch_WhenCalled_With_An_Invalid_DeckThumbnail_Should_Return_BadRequestObjectResult()
        {
            // Arrange
            const string expected = "File is required";

            var thumbnail = new UpdateDeckThumbnailInputModel();

            _sut.ModelState.AddModelError("File", "File is required");

            // Act
            var result = await _sut.Patch(thumbnail) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }
        [Test]
        public async Task Patch_WhenCalled_With_A_Valid_DeckThumbnail_And_Authenticated_User_Is_Not_Found_Should_Return_BadRequestResult()
        {
            var formFile = Substitute.For<IFormFile>();
            var content = new byte[] { 1, 2, 3, 4 };
            const string fileName = "thumbnail.png";

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

            var deck = new UpdateDeckThumbnailInputModel { File = formFile };

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns((ApplicationUser)null);

            // Act
            var result = await _sut.Patch(deck) as BadRequestResult;

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task Patch_WhenCalled_With_A_Valid_DeckThumbnail_If_Deck_Is_Updated_Should_Return_OkObjectResult()
        {
            var formFile = Substitute.For<IFormFile>();
            var content = new byte[] { 1, 2, 3, 4 };
            const string fileName = "thumbnail.png";

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

            var deck = new UpdateDeckThumbnailInputModel { File = formFile, DeckId = 2342 };

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns(new ApplicationUser { Id = Guid.NewGuid().ToString() });
            _mediator.Send(Arg.Any<IRequest<CommandResult>>()).Returns(new CommandResult { IsSuccessful = true, Data = 2342 /*updated deck Id returned*/ });

            // Act
            var result = await _sut.Patch(deck) as OkObjectResult;

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Patch_WhenCalled_With_An_Valid_DeckThumbnail_But_Fails_Validation_Rules_Should_Return_BadRequestObjectResult()
        {
            // Arrange
            const string expected = "deck not found";

            var formFile = Substitute.For<IFormFile>();
            var content = new byte[] { 1, 2, 3, 4 };
            const string fileName = "thumbnail.png";

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

            var deck = new UpdateDeckThumbnailInputModel { File = formFile, DeckId = 2342 };

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns(new ApplicationUser { Id = Guid.NewGuid().ToString() });
            _mediator.Send(Arg.Any<IRequest<CommandResult>>()).Returns(new CommandResult { IsSuccessful = false, Errors = new List<string>{ "deck not found" }  /*updated deck Id returned*/ });

            // Act
            var result = await _sut.Patch(deck) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }


    }
}