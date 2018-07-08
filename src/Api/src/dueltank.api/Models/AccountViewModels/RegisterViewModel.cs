using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        [RegularExpression(@"(^[\w]+$)", ErrorMessage = "Only letters and numbers")]
        [Remote("VerifyUsername", "Accounts")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [Remote("VerifyEmail", "Accounts")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}