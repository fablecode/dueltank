using System.Security.Claims;
using System.Threading.Tasks;
using dueltank.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace dueltank.api.Auth.Claims
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public AppClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
            , IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        { }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            if (!string.IsNullOrWhiteSpace(user.FullName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(ClaimTypes.Name, user.FullName)
                });
            }

            if (!string.IsNullOrWhiteSpace(user.ProfileImageUrl))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim("profile-image-url", user.ProfileImageUrl)
                });
            }


            return principal;
        }
    }
}