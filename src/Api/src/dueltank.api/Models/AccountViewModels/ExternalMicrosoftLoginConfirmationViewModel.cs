using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Models.AccountViewModels
{
    public class ExternalMicrosoftLoginConfirmationViewModel : ExternalLoginConfirmationViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Token { get; set; }
    }
}