using dueltank.api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace dueltank.api.Helpers
{
    public static class UrlHelpers
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountsController.ConfirmEmail),
                controller: "Accounts",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountsController.ResetPassword),
                controller: "Accounts",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}