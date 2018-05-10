using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace dueltank.api.Models.QueryParameters
{
    public class ConfirmEmailQueryParameters
    {
        [BindRequired]
        public string UserId { get; set; }

        [BindRequired]
        public string Code { get; set; }

        [BindRequired]
        [Url]
        public string ReturnUrl { get; set; }
    }
}