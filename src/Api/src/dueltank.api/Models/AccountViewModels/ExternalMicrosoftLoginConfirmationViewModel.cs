using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Models.AccountViewModels
{
    public class ExternalMicrosoftLoginConfirmationViewModel : ExternalLoginConfirmationViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string AccessToken { get; set; }
    }
}