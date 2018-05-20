using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace dueltank.api.Models.QueryParameters
{
    public class ResetPasswordQueryParameters
    {
        [BindRequired]
        [FromQuery]
        public string Code { get; set; }
    }
}