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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace dueltank.api.unit.tests.ControllerTests.AccountsControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ExternalLoginTests
    {
        private UserManager<ApplicationUser> _userManager;
        private IMediator _mediator;
        private AccountsController _sut;
        private IOptions<JwtSettings> _jwtSettings;
        private SignInManager<ApplicationUser> _signInManager;

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

            var roleManager = Substitute.For<RoleManager<IdentityRole>>
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
            _sut = new AccountsController(_userManager, _signInManager, roleManager, logger, _mediator, _jwtSettings);
        }

        [Test]
        public void Given_Valid_Url_Query_Parameters_Should_Return_ChallengeResult()
        {
            // Arrange
            var provider = "facebook";
            var returnUrl = "/home";
            var loginUrl = "/login";
            var lockoutUrl = "/lockout";
            var externalLoginUrl = "http://www.facebook.com/login";
            var externalLoginCompleteUrl = "http://www.dueltank.com";

            var urlHelper = Substitute.For<IUrlHelper>();
            urlHelper.Action(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<object>(), Arg.Any<string>()).Returns("callback");

            _sut.Url = urlHelper;

            // Act
            var result = _sut.ExternalLogin(provider, returnUrl, loginUrl, lockoutUrl, externalLoginUrl, externalLoginCompleteUrl) as ChallengeResult;

            // Assert
            result.Should().BeOfType<ChallengeResult>();
        }

        [Test]
        public void Given_Valid_Url_Query_Parameters_Should_ConfigureExternalAuthenticationProperties_Method_Once()
        {
            // Arrange
            const string provider = "facebook";
            const string returnUrl = "/home";
            const string loginUrl = "/login";
            const string lockoutUrl = "/lockout";
            const string externalLoginUrl = "http://www.facebook.com/login";
            const string externalLoginCompleteUrl = "http://www.dueltank.com";

            var urlHelper = Substitute.For<IUrlHelper>();
            urlHelper.Action(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<object>(), Arg.Any<string>()).Returns("callback");

            _sut.Url = urlHelper;

            // Act
            _sut.ExternalLogin(provider, returnUrl, loginUrl, lockoutUrl, externalLoginUrl, externalLoginCompleteUrl);

            // Assert
            _signInManager.Received(1).ConfigureExternalAuthenticationProperties(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}