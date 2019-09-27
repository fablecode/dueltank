using System;
using dueltank.Configuration;
using dueltank.ViewModels.Infrastructure.ViewModels;
using Reactive.Bindings;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;
using dueltank.ViewModels.Infrastructure.Common;
using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.Helpers;
using Reactive.Bindings.Extensions;

namespace dueltank.ViewModels.Dialogs
{
    public class UsernameContentDialogViewModel : ViewModelBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private UserInfo _userInfo;

        [Required(ErrorMessage = "The username is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [RegularExpression(@"(^[\w]+$)", ErrorMessage = "Only letters and numbers")]
        public ReactiveProperty<string> Username { get; }

        public ReactiveProperty<string> UsernameErrorMessage { get; set; }

        public ReactiveProperty<bool> FormHasErrors { get; }

        public UsernameContentDialogViewModel()
        : this(ServiceLocator.Current.GetService<IHttpClientFactory>())
        {
        }

        public UsernameContentDialogViewModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            Username = new ReactiveProperty<string>(mode: ReactivePropertyMode.Default | ReactivePropertyMode.IgnoreInitialValidationError)
                .SetValidateAttribute(() => Username);

            // You can combine some ObserveHasErrors values.
            FormHasErrors = new[]
                {
                    Username.ObserveHasErrors,
                }
                .CombineLatest(x => !x.Any(y => y))
                .ToReactiveProperty();

            UsernameErrorMessage = Username.ObserveErrorChanged
                .Select(x => x?.OfType<string>().FirstOrDefault())
                .ToReactiveProperty();
        }

        private async Task<string> IsUsernameAvailable(string username)
        {
            return await DispatcherHelper.ExecuteOnUIThreadAsync<string>(async () =>
            {
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    var usernameResult = await httpClient.GetAsync($"https://api.dueltank.com/api/Accounts/VerifyUsername?username={username}");
                    return await usernameResult.Content.ReadAsStringAsync();
                }
            });
        }

        public void LoadUserInfo(UserInfo userInfo)
        {
            _userInfo = userInfo;
        }
    }
}