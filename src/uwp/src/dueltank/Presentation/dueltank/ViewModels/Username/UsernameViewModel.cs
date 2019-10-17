﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;
using dueltank.Configuration;
using dueltank.ViewModels.Infrastructure.Common;
using dueltank.ViewModels.Infrastructure.ViewModels;
using Microsoft.Toolkit.Uwp.Helpers;
using Reactive.Bindings;

namespace dueltank.ViewModels.Username
{
    public class UsernameViewModel : ViewModelBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private UserInfo _userInfo;

        [Required(ErrorMessage = "The username is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [RegularExpression(@"(^[\w]+$)", ErrorMessage = "Only letters and numbers")]
        public ReactiveProperty<string> Username { get; }

        public ReactiveProperty<string> UsernameErrorMessage { get; set; }

        public ReactiveProperty<bool> FormHasErrors { get; }

        public ReactiveCommand RegisterUserCommand { get; }


        public UsernameViewModel()
        : this(ServiceLocator.Current.GetService<IHttpClientFactory>())
        {
        }

        public UsernameViewModel(IHttpClientFactory httpClientFactory)
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

            RegisterUserCommand = new ReactiveCommand();
            RegisterUserCommand.Subscribe(_ =>
            {
                var isValidUsername = IsUsernameAvailable(Username.Value).GetAwaiter();
            });
        }

        private async Task<bool> IsUsernameAvailable(string username)
        {
            return await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    var usernameResult = await httpClient.GetAsync($"http://localhost:56375/api/Accounts/VerifyUsername?username={username}");

                    return bool.TryParse(await usernameResult.Content.ReadAsStringAsync(), out _);
                }
            });
        }

        public void LoadUserInfo(UserInfo userInfo)
        {
            _userInfo = userInfo;
        }
    }
}