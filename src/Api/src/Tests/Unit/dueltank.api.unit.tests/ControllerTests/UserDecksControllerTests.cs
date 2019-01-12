using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dueltank.api.Controllers;
using dueltank.api.Models;
using dueltank.application.Commands;
using dueltank.application.Models.Decks.Input;
using dueltank.application.Models.Decks.Output;
using dueltank.tests.core;
using FluentAssertions;
using MediatR;
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
    public class UserDecksControllerTests
    {
        private IMediator _mediator;
        private UserManager<ApplicationUser> _userManager;
        private UserDecksController _sut;
        private IMapper _mapper;

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

            _mapper = Substitute.For<IMapper>();

            _sut = new UserDecksController(_userManager, _mapper, _mediator);
        }

        [Test]
        public async Task Get_WhenCalled_And_User_Is_Not_Found_Should_Return_BadRequestResult()
        {
            // Arrange
            var searchCriteria = new SearchDecksInputModel();

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns((ApplicationUser)null);

            // Act
            var result = await _sut.Get(searchCriteria) as BadRequestResult;

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task Get_WhenCalled_And_User_Is_Found_Should_Return_OkObjectResult()
        {
            // Arrange
            var searchCriteria = new SearchDecksInputModel();

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns(new ApplicationUser { Id = Guid.NewGuid().ToString()});
            _mapper.Map<DecksByUserIdInputModel>(Arg.Any<SearchDecksInputModel>()).Returns(new DecksByUserIdInputModel());
            _mediator.Send(Arg.Any<IRequest<DeckSearchResultOutputModel>>()).Returns(new DeckSearchResultOutputModel());

            // Act
            var result = await _sut.Get(searchCriteria) as OkObjectResult;

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Delete_WhenCalled_With_A_DeckId_And_User_Is_Not_Found_Should_Return_BadRequestResult()
        {
            // Arrange
            const int deckId = 2342;

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns((ApplicationUser) null);

            // Act
            var result = await _sut.Delete(deckId) as BadRequestResult;

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task Delete_WhenCalled_With_A_DeckId_And_Deck_Deletion_Fails_Should_Return_BadRequestObjectResult()
        {
            // Arrange
            const string expected = "Insufficient permissions to delete this deck.";

            const int deckId = 2342;

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns(new ApplicationUser { Id = Guid.NewGuid().ToString() });
            _mediator.Send(Arg.Any<IRequest<CommandResult>>()).Returns(new CommandResult{ Errors = new List<string> { "Insufficient permissions to delete this deck." } });

            // Act
            var result = await _sut.Delete(deckId) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

        [Test]
        public async Task Delete_WhenCalled_With_A_DeckId_And_Deck_Deletion_Succeeds_Should_Return_OkObjectResult()
        {
            // Arrange
            const int deckId = 2342;

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns(new ApplicationUser { Id = Guid.NewGuid().ToString() });
            _mediator.Send(Arg.Any<IRequest<CommandResult>>()).Returns(new CommandResult{ IsSuccessful = true, Data = 2342 });

            // Act
            var result = await _sut.Delete(deckId) as OkObjectResult;

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}