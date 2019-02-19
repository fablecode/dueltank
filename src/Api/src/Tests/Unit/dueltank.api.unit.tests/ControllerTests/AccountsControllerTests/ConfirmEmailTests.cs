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
    public class ConfirmEmailTests
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
            var confirmEmailQueryParameters = new ConfirmEmailQueryParameters();
            _sut.ModelState.AddModelError("UserId", "UserId is required");

            // Act
            var result = await _sut.ConfirmEmail(confirmEmailQueryParameters);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public void Given_A_Valid_ModelState_If_User_Is_Not_Found_Should_Throw_ApplicationException()
        {
            // Arrange
            var confirmEmailQueryParameters = new ConfirmEmailQueryParameters
            {
                Code = "sdfsfsoifl",
                ReturnUrl = "/",
                UserId = Guid.NewGuid().ToString()
            };

            // Act
            Func<Task<IActionResult>> act = () =>  _sut.ConfirmEmail(confirmEmailQueryParameters);

            // Assert
            act.Should().Throw<ApplicationException>();
        }

        [Test]
        public async Task Given_A_Valid_ModelState_If_User_Is_Found_And_Email_Confirmation_Fails_Should_Return_BadRequest()
        {
            // Arrange
            var confirmEmailQueryParameters = new ConfirmEmailQueryParameters
            {
                Code = "sdfsfsoifl",
                ReturnUrl = "/",
                UserId = Guid.NewGuid().ToString()
            };

            _userManager.FindByIdAsync(Arg.Any<string>()).Returns(new ApplicationUser());
            _userManager.ConfirmEmailAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Failed(new IdentityError { Description = "Code is Invalid"}));

            // Act
           var result = await _sut.ConfirmEmail(confirmEmailQueryParameters);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Given_A_Valid_ModelState_If_User_Is_Found_And_Email_Confirmation_Succeeds_Should_Return_RedirectResult()
        {
            // Arrange
            var confirmEmailQueryParameters = new ConfirmEmailQueryParameters
            {
                Code = "sdfsfsoifl",
                ReturnUrl = "/",
                UserId = Guid.NewGuid().ToString()
            };

            _userManager.FindByIdAsync(Arg.Any<string>()).Returns(new ApplicationUser());
            _userManager.ConfirmEmailAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);

            // Act
           var result = await _sut.ConfirmEmail(confirmEmailQueryParameters);

            // Assert
            result.Should().BeOfType<RedirectResult>();
        }
    }
}