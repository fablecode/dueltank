using dueltank.api.Models;
using dueltank.api.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace dueltank.api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountsController> _logger;
        private readonly IConfiguration _config;

        public AccountsController
        (
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            ILogger<AccountsController> logger,
            IConfiguration config
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
        }
        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Authenticate an existing user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            throw new NotImplementedException();
            //if (ModelState.IsValid)
            //{
            //    // This doesn't count login failures towards account lockout
            //    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            //    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByEmailAsync(model.Email);
            //        _logger.LogInformation($"User {user.Email} logged in.");
            //        var token = await BuildToken(user);
            //        return Ok(new
            //        {
            //            token = token,

            //        });
            //    }
            //}

            //return BadRequest();
        }


        /// <summary>
        /// Logout an authenticated user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<IActionResult> Logout()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Authenticate using a social login
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ExternalLogin([FromQuery]string provider, [FromQuery]string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Accounts", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        /// <summary>
        /// Social login callback
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="remoteError"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (Uri.TryCreate(returnUrl, UriKind.Absolute, out var returnUri))
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }

                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                var profileImage = info.Principal.FindFirstValue("profile-image-url");

                if (info.LoginProvider.Equals("Facebook", StringComparison.OrdinalIgnoreCase))
                {
                    var claim = info.Principal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                    profileImage = "http://graph.facebook.com/" + claim.Value + "/picture?width=200&height=200";
                }


                var user = new ApplicationUser { UserName = email, Email = email, FullName = name, ProfileImageUrl = profileImage };


                // Sign in the user with this external login provider if the user already has a login.
                var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
                if (signInResult.Succeeded)
                {
                    _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);

                    // Append token to returnUrl
                    returnUrl = AppendTokenToReturnUrl(returnUrl, user);

                    return Redirect(returnUrl);
                }

                // Is the user locked out?
                if (signInResult.IsLockedOut)
                {
                    return Redirect(returnUri.Host + "/accounts/lockout");
                }
                
                // create new user
                var identityResult = await _userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    // Add new user to default role
                    await _userManager.AddToRoleAsync(user, "User");

                    // add login
                    identityResult = await _userManager.AddLoginAsync(user, info);
                    if (identityResult.Succeeded)
                    {
                        // sign in new user
                        await _signInManager.SignInAsync(user, false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        // Append token to returnUrl
                        returnUrl = AppendTokenToReturnUrl(returnUrl, user);

                        return Redirect(returnUrl);
                        
                    }

                    throw new ApplicationException($"Error creating an account for {email} using {info.LoginProvider}.");
                }
            }

            throw new UriFormatException("Invalid returnUrl url. ReturnUrl should be an absolute url, not relative.");
        }

        /// <summary>
        /// Get user profile data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByIdAsync(User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                User.Identity.IsAuthenticated,
                user.Id,
                Name = user.FullName,
                user.ProfileImageUrl
            });
        }

        #region private helpers

        private string AppendTokenToReturnUrl(string returnUrl, ApplicationUser user)
        {
            var token = BuildToken(user);
            return QueryHelpers.AddQueryString(returnUrl, "token", token);
        }

        private string BuildToken(ApplicationUser user)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.GivenName, user.FullName),
                new Claim("profile-image-url", user.ProfileImageUrl),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
