using dueltank.api.Controllers;
using dueltank.api.Models;
using dueltank.api.Models.AccountViewModels;
using dueltank.application.Configuration;
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
using System.Security.Claims;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace dueltank.api.unit.tests.ControllerTests.AccountsControllerTests
{
    public class LoginTests
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
            var loginViewModel = new LoginViewModel();
            _sut.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await _sut.Login(loginViewModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Given_An_Invalid_ModelState_Should_Return_BadRequestResult_With_Errors()
        {
            // Arrange
            const string expected = "Email is required";

            var loginViewModel = new LoginViewModel();
            _sut.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await _sut.Login(loginViewModel) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

        [Test]
        public async Task Given_Login_Credentials_If_User_Is_Not_Found_Should_Return_BadRequest()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                Email = "user@social.com",
                Password = "Password",
            };

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns((ApplicationUser) null);

            // Act
            var result = await _sut.Login(loginViewModel) as BadRequestResult;

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task Given_Invalid_Login_Credentials_Should_Return_BadRequest()
        {
            // Arrange
            const string expected = "Invalid login attempt.";
            var loginViewModel = new LoginViewModel
            {
                Email = "user@social.com",
                Password = "Password",
            };

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser
            {
                UserName = "GreatUser"
            });

            _signInManager.PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(SignInResult.Failed);

            // Act
            var result = await _sut.Login(loginViewModel) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

        [Test]
        public async Task Given_Valid_Login_Credentials_User_If_lockedOut_Should_Return_BadRequest()
        {
            // Arrange
            const string expected = "User account locked out.";
            var loginViewModel = new LoginViewModel
            {
                Email = "user@social.com",
                Password = "Password",
            };

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser
            {
                UserName = "GreatUser"
            });

            _signInManager.PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(SignInResult.LockedOut);

            // Act
            var result = await _sut.Login(loginViewModel) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

        [Test]
        public async Task Given_Valid_Login_Credentials_Should_Return_OkResult()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                Email = "user@social.com",
                Password = "Password",
            };

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FullName = "Mr Bean",
                UserName = "GreatUser"
            });

            _signInManager.PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(SignInResult.Success);

            _jwtSettings.Value.Returns(new JwtSettings { Key = "*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI", Issuer = "issue" });
            _userManager.GetClaimsAsync(Arg.Any<ApplicationUser>()).Returns(new List<Claim>());
            _userManager.GetRolesAsync(Arg.Any<ApplicationUser>()).Returns(new List<string>());

            _userManager.GenerateEmailConfirmationTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            var result = await _sut.Login(loginViewModel) as OkObjectResult;

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Given_Valid_Login_Credentials_Should_Invoke_FindByEmailAsync_Once()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                Email = "user@social.com",
                Password = "Password",
            };

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FullName = "Mr Bean",
                UserName = "GreatUser"
            });

            _signInManager.PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(SignInResult.Success);

            _jwtSettings.Value.Returns(new JwtSettings { Key = "*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI", Issuer = "issue" });
            _userManager.GetClaimsAsync(Arg.Any<ApplicationUser>()).Returns(new List<Claim>());
            _userManager.GetRolesAsync(Arg.Any<ApplicationUser>()).Returns(new List<string>());

            _userManager.GenerateEmailConfirmationTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            await _sut.Login(loginViewModel);

            // Assert
            await _userManager.Received(1).FindByEmailAsync(Arg.Any<string>());
        }

        [Test]
        public async Task Given_Valid_Login_Credentials_Should_Invoke_PasswordSignInAsync_Once()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                Email = "user@social.com",
                Password = "Password",
            };

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FullName = "Mr Bean",
                UserName = "GreatUser"
            });

            _signInManager.PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(SignInResult.Success);

            _jwtSettings.Value.Returns(new JwtSettings { Key = "*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI", Issuer = "issue" });
            _userManager.GetClaimsAsync(Arg.Any<ApplicationUser>()).Returns(new List<Claim>());
            _userManager.GetRolesAsync(Arg.Any<ApplicationUser>()).Returns(new List<string>());

            _userManager.GenerateEmailConfirmationTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            await _sut.Login(loginViewModel);

            // Assert
            await _signInManager.Received(1).PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>());
        }

        [Test]
        public async Task Given_Valid_Login_Credentials_Should_Invoke_GetClaimsAsync_Once()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                Email = "user@social.com",
                Password = "Password",
            };

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FullName = "Mr Bean",
                UserName = "GreatUser"
            });

            _signInManager.PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(SignInResult.Success);

            _jwtSettings.Value.Returns(new JwtSettings { Key = "*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI", Issuer = "issue" });
            _userManager.GetClaimsAsync(Arg.Any<ApplicationUser>()).Returns(new List<Claim>());
            _roleManager.GetClaimsAsync(Arg.Any<IdentityRole>()).Returns(new List<Claim>{ new Claim("int", "3242")});
            _userManager.GetRolesAsync(Arg.Any<ApplicationUser>()).Returns(new List<string>{ "Manager"});

            _userManager.GenerateEmailConfirmationTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            await _sut.Login(loginViewModel);

            // Assert
            await _userManager.Received(1).GetClaimsAsync(Arg.Any<ApplicationUser>());
        }

        [Test]
        public async Task Given_Valid_Login_Credentials_Should_Invoke_GetRolesAsync_Once()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                Email = "user@social.com",
                Password = "Password",
            };

            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FullName = "Mr Bean",
                UserName = "GreatUser"
            });

            _signInManager.PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(SignInResult.Success);

            _jwtSettings.Value.Returns(new JwtSettings { Key = "*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI", Issuer = "issue" });
            _userManager.GetClaimsAsync(Arg.Any<ApplicationUser>()).Returns(new List<Claim>());
            _roleManager.GetClaimsAsync(Arg.Any<IdentityRole>()).Returns(new List<Claim>{ new Claim("int", "3242")});
            _userManager.GetRolesAsync(Arg.Any<ApplicationUser>()).Returns(new List<string>{ "Manager"});

            _userManager.GenerateEmailConfirmationTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            await _sut.Login(loginViewModel);

            // Assert
            await _userManager.Received(1).GetRolesAsync(Arg.Any<ApplicationUser>());
        }
    }
}