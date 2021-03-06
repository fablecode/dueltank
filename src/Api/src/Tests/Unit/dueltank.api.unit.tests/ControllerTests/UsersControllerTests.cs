﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dueltank.api.Controllers;
using dueltank.api.Models;
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
    public class UsersControllerTests
    {
        private IMediator _mediator;
        private UserManager<ApplicationUser> _userManager;
        private IMapper _mapper;
        private UsersController _sut;

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

            _sut = new UsersController(_userManager, _mapper, _mediator);
        }

        [Test]
        public async Task Get_WhenCalled_For_AuthenticatedUser_And_User_Is_Not_Found_Should_Return_BadRequestResult()
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

            _userManager.GetUserAsync(Arg.Any<ClaimsPrincipal>()).Returns(new ApplicationUser { Id = Guid.NewGuid().ToString() });
            _mapper.Map<DecksByUserIdInputModel>(Arg.Any<SearchDecksInputModel>()).Returns(new DecksByUserIdInputModel());
            _mediator.Send(Arg.Any<IRequest<DeckSearchResultOutputModel>>()).Returns(new DeckSearchResultOutputModel());

            // Act
            var result = await _sut.Get(searchCriteria) as OkObjectResult;

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }


    }
}