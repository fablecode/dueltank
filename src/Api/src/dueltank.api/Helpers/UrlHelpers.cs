using System.Collections.Specialized;
using System.Linq;
using dueltank.api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace dueltank.api.Helpers
{
    public static class UrlHelpers
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme, string returnUrl)
        {
            return urlHelper.Action(
                action: nameof(AccountsController.ConfirmEmail),
                controller: "Accounts",
                values: new { userId, code, returnUrl },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme, string returnUrl)
        {
            return urlHelper.Action(
                action: nameof(AccountsController.ResetPassword),
                controller: "Accounts",
                values: new { userId, code, returnUrl },
                protocol: scheme);
        }

        public static string AppendToReturnUrl(string returnUrl, NameValueCollection parameters)
        {
            return parameters.AllKeys.Aggregate(returnUrl, (current, key) => QueryHelpers.AddQueryString(current, key, parameters[key]));
        }

    }
}