using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace dueltank.api.Models.QueryParameters
{
    public class RegisterQueryParameters
    {
        [BindRequired]
        [StringLength(2083, MinimumLength = 4)]
        [Url]
        public string ReturnUrl { get; set; }
    }
}