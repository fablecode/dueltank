using dueltank.api.Controllers;
using dueltank.api.Models;
using dueltank.api.Models.AccountViewModels;
using dueltank.application.Commands.SendResetPasswordEmailPassword;
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
using System.Threading.Tasks;

namespace dueltank.api.unit.tests.ControllerTests.AccountsControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ForgotPasswordTests
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
        public async Task Given_An_Invalid_ModelState_Should_Return_BadRequestResult()
        {
            // Arrange
            var forgotPasswordViewModel = new ForgotPasswordViewModel();
            _sut.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await _sut.ForgotPassword(forgotPasswordViewModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Given_An_Invalid_ModelState_Should_Return_BadRequestResult_With_Errors()
        {
            // Arrange
            const string expected = "Email is required";

            var registerViewModel = new ForgotPasswordViewModel();
            _sut.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await _sut.ForgotPassword(registerViewModel) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

        [Test]
        public async Task Given_A_Valid_Email_If_User_Does_Not_Exist_Should_Return_OkResult()
        {
            // Arrange
            var forgotPasswordViewModel = new ForgotPasswordViewModel
            {
                Email = "user@social.com",
            };

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns((ApplicationUser) null);

            // Act
            var result = await _sut.ForgotPassword(forgotPasswordViewModel) as OkResult;

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task Given_A_Valid_Email_If_User_Exists_Should_Return_OkResult()
        {
            // Arrange
            var forgotPasswordViewModel = new ForgotPasswordViewModel
            {
                Email = "user@social.com",
                ResetPasswordConfirmationUrl = "http://www.dueltank.com/reset/password"
            };

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser());
            _userManager.IsEmailConfirmedAsync(Arg.Any<ApplicationUser>()).Returns(true);
            _userManager.GeneratePasswordResetTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            var result = await _sut.ForgotPassword(forgotPasswordViewModel) as OkResult;

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task Given_A_Valid_Email_If_User_Exists_Should_Invoke_FindByEmailAsync_Once()
        {
            // Arrange
            var forgotPasswordViewModel = new ForgotPasswordViewModel
            {
                Email = "user@social.com",
                ResetPasswordConfirmationUrl = "http://www.dueltank.com/reset/password"
            };

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser());
            _userManager.IsEmailConfirmedAsync(Arg.Any<ApplicationUser>()).Returns(true);
            _userManager.GeneratePasswordResetTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            await _sut.ForgotPassword(forgotPasswordViewModel);

            // Assert
            await _userManager.Received(1).FindByEmailAsync(Arg.Any<string>());
        }

        [Test]
        public async Task Given_A_Valid_Email_If_User_Exists_Should_Invoke_IsEmailConfirmedAsync_Once()
        {
            // Arrange
            var forgotPasswordViewModel = new ForgotPasswordViewModel
            {
                Email = "user@social.com",
                ResetPasswordConfirmationUrl = "http://www.dueltank.com/reset/password"
            };

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser());
            _userManager.IsEmailConfirmedAsync(Arg.Any<ApplicationUser>()).Returns(true);
            _userManager.GeneratePasswordResetTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            await _sut.ForgotPassword(forgotPasswordViewModel);

            // Assert
            await _userManager.Received(1).IsEmailConfirmedAsync(Arg.Any<ApplicationUser>());
        }

        [Test]
        public async Task Given_A_Valid_Email_If_User_Exists_Should_Invoke_GeneratePasswordResetTokenAsync_Once()
        {
            // Arrange
            var forgotPasswordViewModel = new ForgotPasswordViewModel
            {
                Email = "user@social.com",
                ResetPasswordConfirmationUrl = "http://www.dueltank.com/reset/password"
            };

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser());
            _userManager.IsEmailConfirmedAsync(Arg.Any<ApplicationUser>()).Returns(true);
            _userManager.GeneratePasswordResetTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            await _sut.ForgotPassword(forgotPasswordViewModel);

            // Assert
            await _userManager.Received(1).GeneratePasswordResetTokenAsync(Arg.Any<ApplicationUser>());
        }


        [Test]
        public async Task Given_A_Valid_Email_If_User_Exists_Should_Invoke_SendResetPasswordEmailPasswordCommand_Once()
        {
            // Arrange
            var forgotPasswordViewModel = new ForgotPasswordViewModel
            {
                Email = "user@social.com",
                ResetPasswordConfirmationUrl = "http://www.dueltank.com/reset/password"
            };

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser());
            _userManager.IsEmailConfirmedAsync(Arg.Any<ApplicationUser>()).Returns(true);
            _userManager.GeneratePasswordResetTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            await _sut.ForgotPassword(forgotPasswordViewModel);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<SendResetPasswordEmailPasswordCommand>());
        }
    }
}