using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using dueltank.api.Controllers;
using dueltank.api.Models;
using dueltank.application.Configuration;
using dueltank.tests.core;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;

namespace dueltank.api.unit.tests.ControllerTests.AccountsControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class VerifyUsernameTests
    {
        private UserManager<ApplicationUser> _userManager;
        private IMediator _mediator;
        private AccountsController _sut;
        private IOptions<JwtSettings> _jwtSettings;
        private SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

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

            _signInManager = Substitute.For<SignInManager<ApplicationUser>>
            (
                _userManager,
                Substitute.For<IHttpContextAccessor>(),
                Substitute.For<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                Substitute.For<IOptions<IdentityOptions>>(),
                Substitute.For<ILogger<SignInManager<ApplicationUser>>>(),
                Substitute.For<IAuthenticationSchemeProvider>()
            );

            _roleManager = Substitute.For<RoleManager<IdentityRole>>
            (
                Substitute.For<IRoleStore<IdentityRole>>(),
                Substitute.For<IEnumerable<IRoleValidator<IdentityRole>>>(),
                Substitute.For<ILookupNormalizer>(),
                Substitute.For<IdentityErrorDescriber>(),
                Substitute.For<ILogger<RoleManager<IdentityRole>>>()
            );

            _mediator = Substitute.For<IMediator>();

            var logger = Substitute.For<ILogger<AccountsController>>();

            _jwtSettings = Substitute.For<IOptions<JwtSettings>>();
            _sut = new AccountsController(_userManager, _signInManager, _roleManager, logger, _mediator, _jwtSettings);
        }

        [Test]
        public async Task Given_A_Username_If_User_Is_Not_Found_Should_Return_User_Details()
        {
            // Arrange
            const string username = "dueltank";

            _userManager.FindByNameAsync(Arg.Any<string>()).Returns(new ApplicationUser());

            // Act
            var result = await _sut.VerifyUsername(username);

            // Assert
            result.Should().BeOfType<JsonResult>();
        }
    }
}