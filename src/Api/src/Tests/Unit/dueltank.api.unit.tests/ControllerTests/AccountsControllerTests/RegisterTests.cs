using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using dueltank.api.Controllers;
using dueltank.api.Models;
using dueltank.api.Models.AccountViewModels;
using dueltank.api.Models.QueryParameters;
using dueltank.application.Commands.SendRegistrationEmail;
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
    public class RegisterTests
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
            var registerViewModel = new RegisterViewModel();
            var registerQueryParameters = new RegisterQueryParameters();
            _sut.ModelState.AddModelError("Username", "Username is required");

            // Act
            var result = await _sut.Register(registerViewModel, registerQueryParameters);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Given_An_Invalid_ModelState_Should_Return_BadRequestResult_With_Errors()
        {
            // Arrange
            const string expected = "Username is required";

            var registerViewModel = new RegisterViewModel();
            var registerQueryParameters = new RegisterQueryParameters();
            _sut.ModelState.AddModelError("Username", "Username is required");

            // Act
            var result = await _sut.Register(registerViewModel, registerQueryParameters) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

        [Test]
        public async Task Given_An_Invalid_ModelState_Should_Not_Invoke_CreateAsync_Method()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel();
            var registerQueryParameters = new RegisterQueryParameters();
            _sut.ModelState.AddModelError("Username", "Username is required");

            // Act
            await _sut.Register(registerViewModel, registerQueryParameters);

            // Assert
            await _userManager.DidNotReceive().CreateAsync(Arg.Any<ApplicationUser>());
        }

        [Test]
        public async Task Given_A_Valid_User_If_User_Already_Exists_Should_Return_BadRequest()
        {
            // Arrange
            const string expected = "User already exists.";
            var registerViewModel = new RegisterViewModel
            {
                Email = "user@social.com",
                Password = "Password",
                Username = "Freezy"
            };

            var registerQueryParameters = new RegisterQueryParameters
            {
                ReturnUrl = "/"
            };

            _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Failed(new IdentityError{ Description = "User already exists."}));

            // Act
            var result = await _sut.Register(registerViewModel, registerQueryParameters) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

        [Test]
        public async Task Given_A_Valid_User_Should_Return_OkResult()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel
            {
                Email = "user@social.com",
                Password = "Password",
                Username = "Freezy"
            };

            var registerQueryParameters = new RegisterQueryParameters
            {
                ReturnUrl = "/"
            };

            _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            var request = Substitute.For<HttpRequest>();
            request.Scheme.Returns("http");
            _sut.ControllerContext = new ControllerContext();
            _sut.ControllerContext.HttpContext = new DefaultHttpContext();

            var urlHelper = Substitute.For<IUrlHelper>();
            urlHelper.Action(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<object>(), Arg.Any<string>()).Returns("callback");

            _sut.Url = urlHelper;

            _jwtSettings.Value.Returns(new JwtSettings {Key = "*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI", Issuer = "issue"});
            _userManager.GetClaimsAsync(Arg.Any<ApplicationUser>()).Returns(new List<Claim>());
            _userManager.GetRolesAsync(Arg.Any<ApplicationUser>()).Returns(new List<string>());

            _userManager.GenerateEmailConfirmationTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            var result = await _sut.Register(registerViewModel, registerQueryParameters) as OkObjectResult;

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task Given_A_Valid_User_Should_Invoke_CreateAsync_Once()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel
            {
                Email = "user@social.com",
                Password = "Password",
                Username = "Freezy"
            };

            var registerQueryParameters = new RegisterQueryParameters
            {
                ReturnUrl = "/"
            };

            _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            var request = Substitute.For<HttpRequest>();
            request.Scheme.Returns("http");
            _sut.ControllerContext = new ControllerContext();
            _sut.ControllerContext.HttpContext = new DefaultHttpContext();

            var urlHelper = Substitute.For<IUrlHelper>();
            urlHelper.Action(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<object>(), Arg.Any<string>()).Returns("callback");

            _sut.Url = urlHelper;

            _jwtSettings.Value.Returns(new JwtSettings {Key = "*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI", Issuer = "issue"});
            _userManager.GetClaimsAsync(Arg.Any<ApplicationUser>()).Returns(new List<Claim>());
            _userManager.GetRolesAsync(Arg.Any<ApplicationUser>()).Returns(new List<string>());

            _userManager.GenerateEmailConfirmationTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            await _sut.Register(registerViewModel, registerQueryParameters);

            // Assert
            await _userManager.Received(1).CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>());
        }

        [Test]
        public async Task Given_A_Valid_User_Should_Invoke_GenerateEmailConfirmationTokenAsync_Once()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel
            {
                Email = "user@social.com",
                Password = "Password",
                Username = "Freezy"
            };

            var registerQueryParameters = new RegisterQueryParameters
            {
                ReturnUrl = "/"
            };

            _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            var request = Substitute.For<HttpRequest>();
            request.Scheme.Returns("http");
            _sut.ControllerContext = new ControllerContext();
            _sut.ControllerContext.HttpContext = new DefaultHttpContext();

            var urlHelper = Substitute.For<IUrlHelper>();
            urlHelper.Action(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<object>(), Arg.Any<string>()).Returns("callback");

            _sut.Url = urlHelper;

            _jwtSettings.Value.Returns(new JwtSettings {Key = "*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI", Issuer = "issue"});
            _userManager.GetClaimsAsync(Arg.Any<ApplicationUser>()).Returns(new List<Claim>());
            _userManager.GetRolesAsync(Arg.Any<ApplicationUser>()).Returns(new List<string>());

            _userManager.GenerateEmailConfirmationTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            await _sut.Register(registerViewModel, registerQueryParameters);

            // Assert
            await _userManager.Received(1).GenerateEmailConfirmationTokenAsync(Arg.Any<ApplicationUser>());
        }

        [Test]
        public async Task Given_A_Valid_User_Should_Invoke_SendRegistrationEmailCommand_Once()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel
            {
                Email = "user@social.com",
                Password = "Password",
                Username = "Freezy"
            };

            var registerQueryParameters = new RegisterQueryParameters
            {
                ReturnUrl = "/"
            };

            _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            var request = Substitute.For<HttpRequest>();
            request.Scheme.Returns("http");
            _sut.ControllerContext = new ControllerContext();
            _sut.ControllerContext.HttpContext = new DefaultHttpContext();

            var urlHelper = Substitute.For<IUrlHelper>();
            urlHelper.Action(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<object>(), Arg.Any<string>()).Returns("callback");

            _sut.Url = urlHelper;

            _jwtSettings.Value.Returns(new JwtSettings {Key = "*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI", Issuer = "issue"});
            _userManager.GetClaimsAsync(Arg.Any<ApplicationUser>()).Returns(new List<Claim>());
            _userManager.GetRolesAsync(Arg.Any<ApplicationUser>()).Returns(new List<string>());

            _userManager.GenerateEmailConfirmationTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            await _sut.Register(registerViewModel, registerQueryParameters);

            // Assert
            await _mediator.Received(1).Send(Arg.Any<SendRegistrationEmailCommand>());
        }

        [Test]
        public async Task Given_A_Valid_User_Should_Invoke_AddToRoleAsync_Once()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel
            {
                Email = "user@social.com",
                Password = "Password",
                Username = "Freezy"
            };

            var registerQueryParameters = new RegisterQueryParameters
            {
                ReturnUrl = "/"
            };

            _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            var request = Substitute.For<HttpRequest>();
            request.Scheme.Returns("http");
            _sut.ControllerContext = new ControllerContext();
            _sut.ControllerContext.HttpContext = new DefaultHttpContext();

            var urlHelper = Substitute.For<IUrlHelper>();
            urlHelper.Action(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<object>(), Arg.Any<string>()).Returns("callback");

            _sut.Url = urlHelper;

            _jwtSettings.Value.Returns(new JwtSettings {Key = "*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI", Issuer = "issue"});
            _userManager.GetClaimsAsync(Arg.Any<ApplicationUser>()).Returns(new List<Claim>());
            _userManager.GetRolesAsync(Arg.Any<ApplicationUser>()).Returns(new List<string>());

            _userManager.GenerateEmailConfirmationTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            await _sut.Register(registerViewModel, registerQueryParameters);

            // Assert
            await _userManager.Received(1).AddToRoleAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>());
        }

        [Test]
        public async Task Given_A_Valid_User_Should_Invoke_SignInAsync_Once()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel
            {
                Email = "user@social.com",
                Password = "Password",
                Username = "Freezy"
            };

            var registerQueryParameters = new RegisterQueryParameters
            {
                ReturnUrl = "/"
            };

            _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            var request = Substitute.For<HttpRequest>();
            request.Scheme.Returns("http");
            _sut.ControllerContext = new ControllerContext();
            _sut.ControllerContext.HttpContext = new DefaultHttpContext();

            var urlHelper = Substitute.For<IUrlHelper>();
            urlHelper.Action(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<object>(), Arg.Any<string>()).Returns("callback");

            _sut.Url = urlHelper;

            _jwtSettings.Value.Returns(new JwtSettings {Key = "*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI", Issuer = "issue"});
            _userManager.GetClaimsAsync(Arg.Any<ApplicationUser>()).Returns(new List<Claim>());
            _userManager.GetRolesAsync(Arg.Any<ApplicationUser>()).Returns(new List<string>());

            _userManager.GenerateEmailConfirmationTokenAsync(Arg.Any<ApplicationUser>()).Returns(@"H!Y7/Q}9I%J61aIn|Z.ouTvY.*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI_|7?");

            // Act
            await _sut.Register(registerViewModel, registerQueryParameters);

            // Assert
            await _signInManager.Received(1).SignInAsync(Arg.Any<ApplicationUser>(), Arg.Any<bool>());
        }
    }
}