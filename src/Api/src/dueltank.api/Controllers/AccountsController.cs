using dueltank.api.Helpers;
using dueltank.api.Models;
using dueltank.api.Models.AccountViewModels;
using dueltank.api.Models.QueryParameters;
using dueltank.api.ServiceExtensions;
using dueltank.application.Commands.SendRegistrationEmail;
using dueltank.application.Commands.SendResetPasswordEmailPassword;
using dueltank.application.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IMediator _mediator;
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController
        (
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountsController> logger,
            IMediator mediator,
            IOptions<JwtSettings> jwtSettings
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _mediator = mediator;
            _jwtSettings = jwtSettings;
        }

        /// <summary>
        ///     Register a new user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model, [FromQuery] RegisterQueryParameters queryParameters)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FullName = model.Username
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl =
                        Url.EmailConfirmationLink(user.Id, code, Request.Scheme, queryParameters.ReturnUrl);
                    await _mediator.Send(new SendRegistrationEmailCommand
                    {
                        Email = model.Email,
                        CallBackUrl = callbackUrl,
                        Username = user.FullName
                    });

                    await _userManager.AddToRoleAsync(user, ApplicationRoles.RoleUser);
                    await _signInManager.SignInAsync(user, false);

                    return Ok(new
                    {
                        token = await BuildToken(user),
                        user = new
                        {
                            user.Id,
                            Name = user.FullName,
                            user.ProfileImageUrl
                        }
                    });
                }

                return BadRequest(result.Errors.Descriptions());
            }

            return BadRequest(ModelState.Errors());
        }

        /// <summary>
        ///     Authenticate an existing user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result =
                        await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, true);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"User {user.Email} logged in.");

                        return Ok(new
                        {
                            token = await BuildToken(user),
                            user = new
                            {
                                user.Id,
                                Name = user.FullName,
                                user.ProfileImageUrl
                            }
                        });
                    }

                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        ModelState.AddModelError(string.Empty, "User account locked out.");
                    }
                    else
                    {
                        _logger.LogWarning("Invalid login attempt.");
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
                }
                else
                {
                    return BadRequest();
                }
            }

            return BadRequest(ModelState.Errors());
        }


        /// <summary>
        ///     Logout an authenticated user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            return Ok();
        }

        /// <summary>
        ///     Send forgotten password email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

                    if (isEmailConfirmed)
                    {
                        var code = WebUtility.UrlEncode(await _userManager.GeneratePasswordResetTokenAsync(user));
                        var callbackUrl = QueryHelpers.AddQueryString(model.ResetPasswordConfirmationUrl,
                            new Dictionary<string, string> {{"code", code}});
                        await _mediator.Send(new SendResetPasswordEmailPasswordCommand
                        {
                            Email = model.Email,
                            CallBackUrl = callbackUrl,
                            Username = user.FullName
                        });
                    }
                }

                // Don't reveal that the user does not exist or is not confirmed
                return Ok();
            }

            return BadRequest(ModelState.Errors());
        }

        /// <summary>
        ///     Reset user password
        /// </summary>
        /// <param name="model"></param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model, [FromQuery] ResetPasswordQueryParameters queryParameters)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null) return Ok();

                var result = await _userManager.ResetPasswordAsync(user, WebUtility.UrlDecode(queryParameters.Code), model.Password);
                if (result.Succeeded) return Ok();

                return BadRequest(result.Errors.Descriptions());
            }

            return BadRequest(ModelState.Errors());
        }

        /// <summary>
        ///     Authenticate using a social login
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <param name="loginUrl"></param>
        /// <param name="lockoutUrl"></param>
        /// <param name="externalLoginUrl"></param>
        /// <param name="externalLoginCompleteUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType((int) HttpStatusCode.Redirect)]
        public IActionResult ExternalLogin([FromQuery] string provider, [FromQuery] string returnUrl,
            [FromQuery] string loginUrl, [FromQuery] string lockoutUrl, [FromQuery] string externalLoginUrl,
            [FromQuery] string externalLoginCompleteUrl)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Accounts",
                new {returnUrl, loginUrl, lockoutUrl, externalLoginUrl, externalLoginCompleteUrl});
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        /// <summary>
        ///     Social login callback
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="loginUrl"></param>
        /// <param name="lockoutUrl"></param>
        /// <param name="externalLoginUrl"></param>
        /// <param name="externalLoginCompleteUrl"></param>
        /// <param name="remoteError"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType((int) HttpStatusCode.Redirect)]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl, string loginUrl, string lockoutUrl,
            string externalLoginUrl, string externalLoginCompleteUrl, string remoteError = null)
        {
            //For "GetExternalLoginInfoAsync" to work on the subsequent request to "ExternalLoginConfirmation", identity cookie must be appended to response.
            //GetExternalLoginInfoAsync is hardcoded to look for the "IdentityConstants.External" cookie, 
            var cookieValueFromReq = Request.Cookies[IdentityConstants.ExternalScheme];
            Response.Cookies.Append(IdentityConstants.ExternalScheme, cookieValueFromReq);

            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null) return Redirect(loginUrl);

            // Sign in the user with this external login provider if the user already has a login.
            var signInResult =
                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true, true);
            if (signInResult.Succeeded)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                var profileImage = info.Principal.FindFirstValue("profile-image-url");

                if (info.LoginProvider.Equals("Facebook", StringComparison.OrdinalIgnoreCase))
                {
                    var claim = info.Principal.Claims.FirstOrDefault(x =>
                        x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                    profileImage = "https://graph.facebook.com/" + claim.Value + "/picture?width=200&height=200";
                }

                var existingUser = await _userManager.FindByEmailAsync(email);

                existingUser.FullName = name;
                existingUser.ProfileImageUrl = profileImage;

                await _userManager.UpdateAsync(existingUser);
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);

                // Append token to returnUrl
                var externalLoginCompleteUrlParameters = new NameValueCollection
                {
                    {"token", await BuildToken(existingUser)}
                };

                if (!string.IsNullOrWhiteSpace(returnUrl))
                    externalLoginCompleteUrlParameters["returnUrl"] = returnUrl;

                externalLoginCompleteUrl =
                    UrlHelpers.AppendToReturnUrl(externalLoginCompleteUrl, externalLoginCompleteUrlParameters);

                return Redirect(externalLoginCompleteUrl);
            }

            // Is the user locked out?
            if (signInResult.IsLockedOut) return Redirect(lockoutUrl);

            var externalLoginUrlParameters = new NameValueCollection
            {
                {"provider", info.LoginProvider}
            };

            if (!string.IsNullOrWhiteSpace(returnUrl))
                externalLoginUrlParameters["returnUrl"] = returnUrl;

            externalLoginUrl = UrlHelpers.AppendToReturnUrl(externalLoginUrl, externalLoginUrlParameters);

            return Redirect(externalLoginUrl);
        }

        /// <summary>
        ///     Social login confirmation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginConfirmation([FromBody] ExternalLoginConfirmationViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _userManager.Users.AnyAsync(u =>
                    u.UserName.Equals(model.Username, StringComparison.OrdinalIgnoreCase)))
                {
                    // Get the information about the user from the external login provider
                    var info = await _signInManager.GetExternalLoginInfoAsync();

                    if (info == null)
                        throw new ApplicationException("Error loading external login information during confirmation.");

                    var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                    var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                    var profileImage = info.Principal.FindFirstValue("profile-image-url");

                    if (info.LoginProvider.Equals("Facebook", StringComparison.OrdinalIgnoreCase))
                    {
                        var claim = info.Principal.Claims.FirstOrDefault(x =>
                            x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                        profileImage = "http://graph.facebook.com/" + claim.Value + "/picture?width=200&height=200";
                    }

                    if (info.LoginProvider.Equals("Microsoft", StringComparison.OrdinalIgnoreCase))
                    {
                        var userId = info.Principal.Claims.First(x =>
                            x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                        profileImage = $"https://apis.live.net/v5.0/{userId}/picture?type=large";
                    }


                    var user = new ApplicationUser
                    {
                        UserName = model.Username,
                        Email = email,
                        FullName = name,
                        ProfileImageUrl = profileImage
                    };

                    // create new user
                    var createIdentityResult = await _userManager.CreateAsync(user);
                    if (createIdentityResult.Succeeded)
                    {
                        // Add new user to default role
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.RoleUser);

                        // add login
                        var addLoginIdentityResult = await _userManager.AddLoginAsync(user, info);
                        if (addLoginIdentityResult.Succeeded)
                        {
                            // sign in new user
                            await _signInManager.SignInAsync(user, false);
                            _logger.LogInformation("User created an account using {Name} provider.",
                                info.LoginProvider);

                            // remove external identity cookie 
                            Response.Cookies.Delete(IdentityConstants.ExternalScheme);

                            return Ok(new
                            {
                                token = await BuildToken(user),
                                user = new
                                {
                                    user.Id,
                                    Name = user.UserName,
                                    user.ProfileImageUrl
                                }
                            });
                        }

                        return BadRequest(addLoginIdentityResult.Errors.Descriptions());
                    }

                    return BadRequest(createIdentityResult.Errors.Descriptions());
                }

                return BadRequest(new[] {$"The username {model.Username} already exists"});
            }

            return BadRequest(ModelState.Errors());
        }

        /// <summary>
        ///     Confirm email address
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQueryParameters queryParameters)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(queryParameters.UserId);

                if (user == null)
                    throw new ApplicationException($"Unable to load user with ID '{queryParameters.UserId}'.");

                var result = await _userManager.ConfirmEmailAsync(user, queryParameters.Code);

                if (result.Succeeded)
                    return Redirect(queryParameters.ReturnUrl);

                return BadRequest(result.Errors.Descriptions());
            }

            return NoContent();
        }


        /// <summary>
        ///     Get user profile data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null) return NotFound();

            return Ok(new
            {
                user.Id,
                Name = user.UserName,
                user.ProfileImageUrl
            });
        }

        /// <summary>
        ///     Check is username already exists
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyUsername(string username)
        {
            //var user = await _userManager.Users.SingleOrDefaultAsync(u =>
            //    u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
            var user = await _userManager.FindByNameAsync(username);

            return user != null ? Json($"Username {username} is already in use.") : Json(true);
        }


        #region private helpers

        private async Task<string> BuildToken(ApplicationUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_jwtSettings.Value.Issuer,
                _jwtSettings.Value.Issuer,
                await GetValidClaims(user),
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<List<Claim>> GetValidClaims(ApplicationUser user)
        {
            var options = new IdentityOptions();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FullName),
                new Claim("profile-image-url", user.ProfileImageUrl ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DatetimeHelpers.ToUnixEpochDate(DateTime.Now),
                    ClaimValueTypes.Integer64),
                new Claim(options.ClaimsIdentity.UserNameClaimType, user.UserName)
            };
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userClaims);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims) claims.Add(roleClaim);
                }
            }

            return claims;
        }

        #endregion
    }
}