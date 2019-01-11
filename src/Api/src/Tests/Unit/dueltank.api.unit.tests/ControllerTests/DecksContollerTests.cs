using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using dueltank.api.Controllers;
using dueltank.api.Models;
using dueltank.application.Commands;
using dueltank.application.Models.Decks.Input;
using dueltank.application.Models.Decks.Output;
using dueltank.tests.core;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace dueltank.api.unit.tests.ControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class DecksContollerTests
    {
        private IMediator _mediator;
        private DecksController _sut;
        private UserManager<ApplicationUser> _userManager;

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
            
            _sut = new DecksController(_mediator, _userManager);
        }

        [Test]
        public async Task Get_WhenCalled_With_An_DeckId_Should_Return_OkResult()
        {
            // Arrange
            const int deckId = 43242;

            _mediator.Send(Arg.Any<IRequest<DeckDetailOutputModel>>()).Returns(new DeckDetailOutputModel());

            // Act
            var result = await _sut.Get(deckId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Get_WhenCalled_With_An_DeckId_If_Deck_Is_Not_Found_Should_Return_NotFoundResult()
        {
            // Arrange
            const int deckId = 43242;

            _mediator.Send(Arg.Any<IRequest<DeckDetailOutputModel>>()).Returns((DeckDetailOutputModel) null);

            // Act
            var result = await _sut.Get(deckId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public async Task Get_MostRecentDecks_WhenCalled_With_An_DeckId_Should_Return_OkResult()
        {
            // Arrange
            const int pageSize = 10;

            _mediator.Send(Arg.Any<IRequest<DeckDetailOutputModel>>()).Returns(new DeckDetailOutputModel());

            // Act
            var result = await _sut.MostRecentDecks(pageSize);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Post_WhenCalled_With_An_Invalid_Deck_Should_Return_BadRequestObjectResult()
        {
            // Arrange
            const string expected = "Name is required";

            var deck = new DeckInputModel();

            _sut.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _sut.Post(deck)as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

        [Test]
        public async Task Post_WhenCalled_With_A_Valid_Deck_And_Authenticated_User_Is_Not_Found_Should_Return_BadRequestResult()
        {
            // Arrange
            var deck = new DeckInputModel { Name = "Test Deck"};

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns((ApplicationUser) null);

            // Act
            var result = await _sut.Post(deck)as BadRequestResult;

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task Post_WhenCalled_With_A_Valid_Deck_If_Deck_Is_Created_Should_Return_CreatedAtRouteResult()
        {
            // Arrange
            var deck = new DeckInputModel { Name = "Test Deck"};

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns(new ApplicationUser { Id = Guid.NewGuid().ToString() });

            _mediator.Send(Arg.Any<IRequest<CommandResult>>()).Returns(new CommandResult{ IsSuccessful = true, Data = 2342 /*New deck Id returned*/ });

            // Act
            var result = await _sut.Post(deck)as CreatedAtRouteResult;

            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>();
        }

        [Test]
        public async Task Post_WhenCalled_With_An_Valid_Deck_But_Fails_Validation_Rules_Should_Return_BadRequestObjectResult()
        {
            // Arrange
            const string expected = "40 cards minimum";
            var deck = new DeckInputModel { Name = "Test Deck" };

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns(new ApplicationUser { Id = Guid.NewGuid().ToString() });

            _mediator.Send(Arg.Any<IRequest<CommandResult>>()).Returns(new CommandResult { Errors = new List<string>{ "40 cards minimum" } });

            // Act
            var result = await _sut.Post(deck) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }
        [Test]
        public async Task Put_WhenCalled_With_An_Invalid_Deck_Should_Return_BadRequestObjectResult()
        {
            // Arrange
            const string expected = "Name is required";

            var deck = new DeckInputModel();

            _sut.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _sut.Put(deck)as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

        [Test]
        public async Task Put_WhenCalled_With_A_Valid_Deck_And_Authenticated_User_Is_Not_Found_Should_Return_BadRequestResult()
        {
            // Arrange
            var deck = new DeckInputModel { Name = "Test Deck"};

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns((ApplicationUser) null);

            // Act
            var result = await _sut.Put(deck)as BadRequestResult;

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task Put_WhenCalled_With_A_Valid_Deck_If_Deck_Is_Created_Should_Return_CreatedAtRouteResult()
        {
            // Arrange
            var deck = new DeckInputModel { Name = "Test Deck"};

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns(new ApplicationUser { Id = Guid.NewGuid().ToString() });

            _mediator.Send(Arg.Any<IRequest<CommandResult>>()).Returns(new CommandResult{ IsSuccessful = true, Data = 2342 });

            // Act
            var result = await _sut.Put(deck)as OkObjectResult;

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Put_WhenCalled_With_An_Valid_Deck_But_Fails_Validation_Rules_Should_Return_BadRequestObjectResult()
        {
            // Arrange
            const string expected = "40 cards minimum";
            var deck = new DeckInputModel { Name = "Test Deck" };

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns(new ApplicationUser { Id = Guid.NewGuid().ToString() });

            _mediator.Send(Arg.Any<IRequest<CommandResult>>()).Returns(new CommandResult { Errors = new List<string>{ "40 cards minimum" } });

            // Act
            var result = await _sut.Put(deck) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

    }
}