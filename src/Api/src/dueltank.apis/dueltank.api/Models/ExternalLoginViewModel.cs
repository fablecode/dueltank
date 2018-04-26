using System.ComponentModel.DataAnnotations;

namespace dueltank.api.Models
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}