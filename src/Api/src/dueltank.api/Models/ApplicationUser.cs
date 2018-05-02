using System;
using Microsoft.AspNetCore.Identity;

namespace dueltank.api.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}