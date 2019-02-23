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
using System.Security.Claims;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace dueltank.api.unit.tests.ControllerTests.AccountsControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ExternalLoginCallbackTests
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
        public async Task Given_Invalid_Login_Info_Should_Return_RedirectResult()
        {
            // Arrange
            const string returnUrl = "/home";
            const string loginUrl = "/login";
            const string lockoutUrl = "/lockoutUrl";
            const string externalLoginUrl = "/externalLoginUrl";
            const string externalLoginCompleteUrl = "/externalLoginCompleteUrl";

            _sut.ControllerContext = new ControllerContext();
            _sut.ControllerContext.HttpContext = new DefaultHttpContext();

            var newCookies = new[] { $"{IdentityConstants.ExternalScheme}=value0%2C", "%5Ename1=value1" };
            _sut.ControllerContext.HttpContext.Request.Headers["Cookie"] = newCookies;

            // Act
            var result = await _sut.ExternalLoginCallback(returnUrl, loginUrl, lockoutUrl, externalLoginUrl, externalLoginCompleteUrl);

            // Assert
            result.Should().BeOfType<RedirectResult>();
        }

        [Test]
        public async Task Given_Invalid_Login_Info_Should_Return_RedirectResult_With_LoginUrl_As_The_Redirect_Url()
        {
            // Arrange
            const string expected = "/login";

            const string returnUrl = "/home";
            const string loginUrl = "/login";
            const string lockoutUrl = "/lockoutUrl";
            const string externalLoginUrl = "/externalLoginUrl";
            const string externalLoginCompleteUrl = "/externalLoginCompleteUrl";

            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var newCookies = new[] { $"{IdentityConstants.ExternalScheme}=value0%2C", "%5Ename1=value1" };
            _sut.ControllerContext.HttpContext.Request.Headers["Cookie"] = newCookies;

            // Act
            var result = await _sut.ExternalLoginCallback(returnUrl, loginUrl, lockoutUrl, externalLoginUrl, externalLoginCompleteUrl) as RedirectResult;

            // Assert
            result.Should().BeOfType<RedirectResult>();
            var resultValue = result?.Url;
            resultValue.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Given_Valid_LoginInfo_If_SignIn_Fails_Should_Return_RedirectResult_With_ExternalLoginUrl_As_The_Redirect_Url()
        {
            // Arrange
            const string expected = "/externalLoginUrl?provider=facebook&returnUrl=%2Fhome";

            const string returnUrl = "/home";
            const string loginUrl = "/login";
            const string lockoutUrl = "/lockoutUrl";
            const string externalLoginUrl = "/externalLoginUrl";
            const string externalLoginCompleteUrl = "/externalLoginCompleteUrl";

            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var newCookies = new[] { $"{IdentityConstants.ExternalScheme}=value0%2C", "%5Ename1=value1" };
            _sut.ControllerContext.HttpContext.Request.Headers["Cookie"] = newCookies;
            _signInManager.GetExternalLoginInfoAsync().Returns(new ExternalLoginInfo(new ClaimsPrincipal(), "facebook", "wersfswe", "facebook"));
            _signInManager.ExternalLoginSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(SignInResult.Failed);

            // Act
            var result = await _sut.ExternalLoginCallback(returnUrl, loginUrl, lockoutUrl, externalLoginUrl, externalLoginCompleteUrl) as RedirectResult;

            // Assert
            result.Should().BeOfType<RedirectResult>();
            var resultValue = result?.Url;
            resultValue.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Given_Valid_LoginInfo_If_SignIn_Succeeds_Should_Return_RedirectResult_With_ExternalLoginUrl_As_The_Redirect_Url()
        {
            // Arrange
            const string expected = "/externalLoginCompleteUrl";

            const string returnUrl = "/home";
            const string loginUrl = "/login";
            const string lockoutUrl = "/lockoutUrl";
            const string externalLoginUrl = "/externalLoginUrl";
            const string externalLoginCompleteUrl = "/externalLoginCompleteUrl";

            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var newCookies = new[] { $"{IdentityConstants.ExternalScheme}=value0%2C", "%5Ename1=value1" };
            _sut.ControllerContext.HttpContext.Request.Headers["Cookie"] = newCookies;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "dueltank"),
                new Claim(ClaimTypes.Email, "tanker@poo.com")
            };

            var identity = new ClaimsIdentity();
            identity.AddClaims(claims);

            var claimsPrincipal = new ClaimsPrincipal(identity);
            _signInManager.GetExternalLoginInfoAsync().Returns(new ExternalLoginInfo(claimsPrincipal, "facebook", "wersfswe", "facebook"));
            _signInManager.ExternalLoginSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(SignInResult.Success);
            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser{ UserName = "dueltank"});
            _jwtSettings.Value.Returns(new JwtSettings { Key = "*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI", Issuer = "issue" });
            _userManager.GetClaimsAsync(Arg.Any<ApplicationUser>()).Returns(claims);
            _userManager.GetRolesAsync(Arg.Any<ApplicationUser>()).Returns(new List<string> {"Admin", "User"});

            // Act
            var result = await _sut.ExternalLoginCallback(returnUrl, loginUrl, lockoutUrl, externalLoginUrl, externalLoginCompleteUrl) as RedirectResult;

            // Assert
            result.Should().BeOfType<RedirectResult>();
            var resultValue = result?.Url;
            resultValue.Should().Contain(expected);
        }

        [Test]
        public async Task Given_Valid_LoginInfo_If_SignIn_Succeeds_GetExternalLoginInfoAsync_Should_Be_Invoked_Once()
        {
            // Arrange
            const string returnUrl = "/home";
            const string loginUrl = "/login";
            const string lockoutUrl = "/lockoutUrl";
            const string externalLoginUrl = "/externalLoginUrl";
            const string externalLoginCompleteUrl = "/externalLoginCompleteUrl";

            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var newCookies = new[] { $"{IdentityConstants.ExternalScheme}=value0%2C", "%5Ename1=value1" };
            _sut.ControllerContext.HttpContext.Request.Headers["Cookie"] = newCookies;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "dueltank"),
                new Claim(ClaimTypes.Email, "tanker@poo.com")
            };

            var identity = new ClaimsIdentity();
            identity.AddClaims(claims);

            var claimsPrincipal = new ClaimsPrincipal(identity);
            _signInManager.GetExternalLoginInfoAsync().Returns(new ExternalLoginInfo(claimsPrincipal, "facebook", "wersfswe", "facebook"));
            _signInManager.ExternalLoginSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(SignInResult.Success);
            _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(new ApplicationUser{ UserName = "dueltank"});
            _jwtSettings.Value.Returns(new JwtSettings { Key = "*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI", Issuer = "issue" });
            _userManager.GetClaimsAsync(Arg.Any<ApplicationUser>()).Returns(claims);
            _userManager.GetRolesAsync(Arg.Any<ApplicationUser>()).Returns(new List<string> {"Admin", "User"});

            // Act
            await _sut.ExternalLoginCallback(returnUrl, loginUrl, lockoutUrl, externalLoginUrl, externalLoginCompleteUrl);

            // Assert
            await _signInManager.Received(1).GetExternalLoginInfoAsync();
        }
    }
}