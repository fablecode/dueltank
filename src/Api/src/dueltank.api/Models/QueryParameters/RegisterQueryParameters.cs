using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace dueltank.api.Models.QueryParameters
{
    public class RegisterQueryParameters
    {
        [BindRequired]
        [Url]
        [StringLength(2083, MinimumLength = 4)]
        public string ReturnUrl { get; set; }
    }
}