using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace dueltank.api.Models.QueryParameters
{
    public class RegisterQueryParameters
    {
        [BindRequired]
        public string ReturnUrl { get; set; }
    }
}