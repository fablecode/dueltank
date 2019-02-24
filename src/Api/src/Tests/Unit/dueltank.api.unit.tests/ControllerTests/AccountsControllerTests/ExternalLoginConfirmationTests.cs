using dueltank.api.Controllers;
using dueltank.api.Models;
using dueltank.api.Models.AccountViewModels;
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
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dueltank.api.unit.tests.ControllerTests.AccountsControllerTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class ExternalLoginConfirmationTests
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
            var externalLoginConfirmationViewModel = new ExternalLoginConfirmationViewModel();
            _sut.ModelState.AddModelError("Username", "Username is required");

            // Act
            var result = await _sut.ExternalLoginConfirmation(externalLoginConfirmationViewModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Given_An_Invalid_ModelState_Should_Return_BadRequestResult_With_Errors()
        {
            // Arrange
            const string expected = "Username is required";
            var externalLoginConfirmationViewModel = new ExternalLoginConfirmationViewModel();
            _sut.ModelState.AddModelError("Username", "Username is required");

            // Act
            var result = await _sut.ExternalLoginConfirmation(externalLoginConfirmationViewModel) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

        [Test]
        public async Task Given_A_Valid_ModelState_If_User_Is_Found_Should_Return_BadRequestResult()
        {
            // Arrange
            const string username = "dueltank";

            var externalLoginConfirmationViewModel = new ExternalLoginConfirmationViewModel
            {
                Username = username
            };

            _userManager.FindByNameAsync(Arg.Any<string>()).Returns(new ApplicationUser());

            // Act
            var result = await _sut.ExternalLoginConfirmation(externalLoginConfirmationViewModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Given_A_Valid_ModelState_If_User_Is_Found_Should_Return_BadRequestResult_With_Errors()
        {
            // Arrange
            const string username = "dueltank";

            var expected = $"The username {username} already exists";

            var externalLoginConfirmationViewModel = new ExternalLoginConfirmationViewModel
            {
                Username = username
            };

            _userManager.FindByNameAsync(Arg.Any<string>()).Returns(new ApplicationUser());

            // Actt
            var result = await _sut.ExternalLoginConfirmation(externalLoginConfirmationViewModel) as BadRequestObjectResult;

            // Assert
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }


        [Test]
        public void Given_A_Valid_ModelState_If_ExternalLoginInfo_Is_Found_Should_Throw_ApplicationException()
        {
            // Arrange
            const string username = "dueltank";

            var externalLoginConfirmationViewModel = new ExternalLoginConfirmationViewModel
            {
                Username = username
            };

            _userManager.FindByNameAsync(Arg.Any<string>()).Returns((ApplicationUser)null);
            _signInManager.GetExternalLoginInfoAsync().Returns((ExternalLoginInfo) null);

            // Act
            Func<Task<IActionResult>> act = () => _sut.ExternalLoginConfirmation(externalLoginConfirmationViewModel);

            // Assert
            act.Should().Throw<ApplicationException>();
        }

        [Test]
        public async Task Given_A_Valid_ModelState_If_User_Creation_Fails_Should_Return_BadRequest()
        {
            // Arrange
            const string username = "dueltank";

            var externalLoginConfirmationViewModel = new ExternalLoginConfirmationViewModel
            {
                Username = username
            };
            _userManager.FindByNameAsync(Arg.Any<string>()).Returns((ApplicationUser)null);

            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var newCookies = new[] { $"{IdentityConstants.ExternalScheme}=value0%2C", "%5Ename1=value1" };
            _sut.ControllerContext.HttpContext.Request.Headers["Cookie"] = newCookies;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "dueltank"),
                new Claim(ClaimTypes.Email, "tanker@poo.com"),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "tanker")
            };

            var identity = new ClaimsIdentity();
            identity.AddClaims(claims);

            var claimsPrincipal = new ClaimsPrincipal(identity);
            _signInManager.GetExternalLoginInfoAsync().Returns(new ExternalLoginInfo(claimsPrincipal, "facebook", "wersfswe", "facebook"));
            _userManager.CreateAsync(Arg.Any<ApplicationUser>()).Returns(IdentityResult.Failed(new IdentityError{ Description = "User already exists"}));

            // Act
            var result = await _sut.ExternalLoginConfirmation(externalLoginConfirmationViewModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Given_A_Valid_ModelState_If_User_Creation_Fails_Should_Return_BadRequest_With_Errors()
        {
            // Arrange
            const string expected = "User already exists";
            const string username = "dueltank";

            var externalLoginConfirmationViewModel = new ExternalLoginConfirmationViewModel
            {
                Username = username
            };
            _userManager.FindByNameAsync(Arg.Any<string>()).Returns((ApplicationUser)null);

            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var newCookies = new[] { $"{IdentityConstants.ExternalScheme}=value0%2C", "%5Ename1=value1" };
            _sut.ControllerContext.HttpContext.Request.Headers["Cookie"] = newCookies;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "dueltank"),
                new Claim(ClaimTypes.Email, "tanker@poo.com"),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "tanker")
            };

            var identity = new ClaimsIdentity();
            identity.AddClaims(claims);

            var claimsPrincipal = new ClaimsPrincipal(identity);
            _signInManager.GetExternalLoginInfoAsync().Returns(new ExternalLoginInfo(claimsPrincipal, "facebook", "wersfswe", "facebook"));
            _userManager.CreateAsync(Arg.Any<ApplicationUser>()).Returns(IdentityResult.Failed(new IdentityError { Description = "User already exists" }));

            // Act
            var result = await _sut.ExternalLoginConfirmation(externalLoginConfirmationViewModel) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var errors = result?.Value as IEnumerable<string>;
            errors.Should().ContainSingle(expected);
        }

        [Test]
        public async Task Given_A_Valid_ModelState_If_User_Creation_Succeeeds_But_User_Not_Added_To_Roles_Should_Return_BadRequest()
        {
            // Arrange
            const string expected = "User not added to role";
            const string username = "dueltank";

            var externalLoginConfirmationViewModel = new ExternalLoginConfirmationViewModel
            {
                Username = username
            };
            _userManager.FindByNameAsync(Arg.Any<string>()).Returns((ApplicationUser)null);

            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var newCookies = new[] { $"{IdentityConstants.ExternalScheme}=value0%2C", "%5Ename1=value1" };
            _sut.ControllerContext.HttpContext.Request.Headers["Cookie"] = newCookies;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "dueltank"),
                new Claim(ClaimTypes.Email, "tanker@poo.com"),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "tanker")
            };

            var identity = new ClaimsIdentity();
            identity.AddClaims(claims);

            var claimsPrincipal = new ClaimsPrincipal(identity);
            _signInManager.GetExternalLoginInfoAsync().Returns(new ExternalLoginInfo(claimsPrincipal, "facebook", "wersfswe", "facebook"));
            _userManager.CreateAsync(Arg.Any<ApplicationUser>()).Returns(IdentityResult.Success);
            _userManager.AddLoginAsync(Arg.Any<ApplicationUser>(), Arg.Any<ExternalLoginInfo>()).Returns(IdentityResult.Failed(new IdentityError {Description = "User not added to role"}));

            // Act
            var result = await _sut.ExternalLoginConfirmation(externalLoginConfirmationViewModel) as BadRequestObjectResult;

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Given_A_Valid_ModelState_If_User_Creation_Succeeeds_But_User_Not_Added_To_Roles_Should_Return_BadRequest_With_Errors()
        {
            // Arrange
            const string expected = "User not added to role";
            const string username = "dueltank";

            var externalLoginConfirmationViewModel = new ExternalLoginConfirmationViewModel
            {
                Username = username
            };
            _userManager.FindByNameAsync(Arg.Any<string>()).Returns((ApplicationUser)null);

            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var newCookies = new[] { $"{IdentityConstants.ExternalScheme}=value0%2C", "%5Ename1=value1" };
            _sut.ControllerContext.HttpContext.Request.Headers["Cookie"] = newCookies;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "dueltank"),
                new Claim(ClaimTypes.Email, "tanker@poo.com"),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "tanker")
            };

            var identity = new ClaimsIdentity();
            identity.AddClaims(claims);

            var claimsPrincipal = new ClaimsPrincipal(identity);
            _signInManager.GetExternalLoginInfoAsync().Returns(new ExternalLoginInfo(claimsPrincipal, "microsoft", "wersfswe", "microsoft"));
            _userManager.CreateAsync(Arg.Any<ApplicationUser>()).Returns(IdentityResult.Success);
            _userManager.AddLoginAsync(Arg.Any<ApplicationUser>(), Arg.Any<ExternalLoginInfo>()).Returns(IdentityResult.Failed(new IdentityError {Description = "User not added to role"}));

            // Act
            var result = await _sut.ExternalLoginConfirmation(externalLoginConfirmationViewModel) as BadRequestObjectResult;

            // Assert
            var errors = result?.Value as IEnumerable<string>;
            errors?.Single().Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task Given_A_Valid_ModelState_If_User_Creation_Succeeeds_But_User_Is_Added_To_Roles_Should_Return_OkResults()
        {
            // Arrange
            const string username = "dueltank";

            var externalLoginConfirmationViewModel = new ExternalLoginConfirmationViewModel
            {
                Username = username
            };
            _userManager.FindByNameAsync(Arg.Any<string>()).Returns((ApplicationUser)null);

            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var newCookies = new[] { $"{IdentityConstants.ExternalScheme}=value0%2C", "%5Ename1=value1" };
            _sut.ControllerContext.HttpContext.Request.Headers["Cookie"] = newCookies;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "dueltank"),
                new Claim(ClaimTypes.Email, "tanker@poo.com"),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "tanker")
            };

            var identity = new ClaimsIdentity();
            identity.AddClaims(claims);

            var claimsPrincipal = new ClaimsPrincipal(identity);
            _signInManager.GetExternalLoginInfoAsync().Returns(new ExternalLoginInfo(claimsPrincipal, "microsoft", "wersfswe", "microsoft"));
            _userManager.CreateAsync(Arg.Any<ApplicationUser>()).Returns(IdentityResult.Success);
            _userManager.AddLoginAsync(Arg.Any<ApplicationUser>(), Arg.Any<ExternalLoginInfo>()).Returns(IdentityResult.Success);
            _jwtSettings.Value.Returns(new JwtSettings { Key = "*@thu}qx)@h0-kI9%$](^l~xuFK>TL,%$EI", Issuer = "issue" });

            // Act
            var result = await _sut.ExternalLoginConfirmation(externalLoginConfirmationViewModel) as OkObjectResult;

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}