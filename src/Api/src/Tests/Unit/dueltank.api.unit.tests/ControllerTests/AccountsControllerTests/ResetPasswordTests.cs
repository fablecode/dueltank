using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.api.Controllers;
using dueltank.api.Models;
using dueltank.api.Models.AccountViewModels;
using dueltank.api.Models.QueryParameters;
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

namespace dueltank.api.unit.tests.ControllerTests.AccountsControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ResetPasswordTests
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
        public async Task Given_An_Invalid_ModelState_Should_Return_BadRequestResult()
        {
            // Arrange
            var resetPasswordViewModel = new ResetPasswordViewModel();
            var resetPasswordQueryParameters = new ResetPasswordQueryParameters();

            _sut.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await _sut.ResetPassword(resetPasswordViewModel, resetPasswordQueryParameters);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Given_An_Invalid_ModelState_Should_Return_BadRequestResult_With_Errors()
        {
            // Arrange
            const string expected = "Email is required";

            var resetPasswordViewModel = new ResetPasswordViewModel();
            var resetPasswordQueryParameters = new ResetPasswordQueryParameters();
            _sut.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await _sut.ResetPassword(resetPasswordViewModel, resetPasswordQueryParameters) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

        [Test]
        public async Task Given_A_Valid_Email_If_User_Does_Not_Exist_Should_Return_OkResult()
        {
            // Arrange
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                Email = "user@social.com",
            };

            var resetPasswordQueryParameters = new ResetPasswordQueryParameters();

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns((ApplicationUser)null);

            // Act
            var result = await _sut.ResetPassword(resetPasswordViewModel, resetPasswordQueryParameters) as OkResult;

            // Assert
            result.Should().BeOfType<OkResult>();
        }


        [Test]
        public async Task Given_A_Valid_ResetPassword_Credentials_If_ResetPassword_Fails_Should_Return_BadRequestResult()
        {
            // Arrange
            const string expected = "Reset password code expired.";

            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                Email = "user@social.com",
                Password = "password"
            };

            var resetPasswordQueryParameters = new ResetPasswordQueryParameters();
            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser());
            _userManager.ResetPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(), Arg.Any<string>()).Returns(IdentityResult.Failed(new IdentityError
            {
                Description = "Reset password code expired."
            }));

            // Act
            var result = await _sut.ResetPassword(resetPasswordViewModel, resetPasswordQueryParameters) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

        [Test]
        public async Task Given_A_Valid_ResetPassword_Credentials_If_ResetPassword_Succeeds_Should_Return_OkResult()
        {
            // Arrange
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                Email = "user@social.com",
                Password = "password"
            };

            var resetPasswordQueryParameters = new ResetPasswordQueryParameters();
            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser());
            _userManager.ResetPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            // Act
            var result = await _sut.ResetPassword(resetPasswordViewModel, resetPasswordQueryParameters) as OkResult;

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task Given_A_Valid_ResetPassword_Credentials_If_ResetPassword_Succeeds_Should_Invoke_FindByEmailAsync_Once()
        {
            // Arrange
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                Email = "user@social.com",
                Password = "password"
            };

            var resetPasswordQueryParameters = new ResetPasswordQueryParameters();
            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser());
            _userManager.ResetPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            // Act
            await _sut.ResetPassword(resetPasswordViewModel, resetPasswordQueryParameters);

            // Assert
            await _userManager.Received(1).FindByEmailAsync(Arg.Any<string>());
        }

        [Test]
        public async Task Given_A_Valid_ResetPassword_Credentials_If_ResetPassword_Succeeds_Should_Invoke_ResetPasswordAsync_Once()
        {
            // Arrange
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                Email = "user@social.com",
                Password = "password"
            };

            var resetPasswordQueryParameters = new ResetPasswordQueryParameters();
            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser());
            _userManager.ResetPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            // Act
            await _sut.ResetPassword(resetPasswordViewModel, resetPasswordQueryParameters);

            // Assert
            await _userManager.Received(1).ResetPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>(), Arg.Any<string>());
        }
    }
}