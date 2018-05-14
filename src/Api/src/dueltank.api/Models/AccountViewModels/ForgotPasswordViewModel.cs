using System.ComponentModel.DataAnnotations;

namespace dueltank.api.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}