using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        [RegularExpression(@"(^[\w]+$)", ErrorMessage = "Only letters and numbers")]
        [Remote("VerifyUsername", "Accounts")]
        public string Username { get; set; }
    }
}