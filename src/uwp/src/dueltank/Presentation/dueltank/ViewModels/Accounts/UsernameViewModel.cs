using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;
using Windows.Storage;
using dueltank.application.Configuration;
using dueltank.Configuration;
using dueltank.ViewModels.Home;
using dueltank.ViewModels.Infrastructure;
using dueltank.ViewModels.Infrastructure.Common;
using dueltank.ViewModels.Infrastructure.Services;
using dueltank.ViewModels.Infrastructure.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using Reactive.Bindings;

namespace dueltank.ViewModels.Accounts
{
    public class UsernameViewModel : ViewModelBase
    {
        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;
        private readonly IHttpClientFactory _httpClientFactory;
        private UserInfo _userInfo;
        private bool _isBusy;
        private string _isBusyText;


        private const string UserInfoLocalStorageKey = "userinfo.json";
        const string StoredAccountIdKey = "accountid";
        const string StoredProviderIdKey = "providerid";
        const string StoredAuthorityKey = "authority";

        const string AppSpecificProviderId = "dueltankproviderid";
        const string AppSpecificProviderName = "App specific provider";



        [Required(ErrorMessage = "The username is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [RegularExpression(@"(^[\w]+$)", ErrorMessage = "Only letters and numbers")]
        public ReactiveProperty<string> Username { get; }

        public ReactiveProperty<string> UsernameErrorMessage { get; set; }

        public ReactiveProperty<bool> FormHasErrors { get; }

        public ReactiveCommand RegisterUserCommand { get; }
        public ReactiveCommand CancelRegisterUserCommand { get; }


        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }
        public string IsBusyText
        {
            get => _isBusyText;
            set => Set(ref _isBusyText, value);
        }



        public UsernameViewModel()
        : this
            (
            ServiceLocator.Current.GetService<IAccountService>(),
            ServiceLocator.Current.GetService<INavigationService>(), 
            ServiceLocator.Current.GetService<IHttpClientFactory>()
            )
        {
        }

        public UsernameViewModel(IAccountService accountService, INavigationService navigationService, IHttpClientFactory httpClientFactory)
        {
            _accountService = accountService;
            _navigationService = navigationService;
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
            RegisterUserCommand.Subscribe(async _ =>
            {
                SetBusyStatus(true, "Getting authentication token...");

                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    var json = JsonConvert.SerializeObject(new { Username = Username.Value, AccessToken = _userInfo.WebTokenRequestResultToken });
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"http://localhost:56375/api/Accounts/ExternalMicrosoftLoginConfirmation", stringContent);
                    var responseResult = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<JwtTokenResult>(responseResult);

                    var jwtToken = tokenResponse.Token;
                    SetBusyStatus(true, "Authentication successful...");
                    _userInfo.JwtToken = jwtToken;
                    SetBusyStatus(true, "Saving authentication details...");

                    await RemoveUserLocalFile();
                    RemoveAccountData();

                    StoreNewAccountDataLocally(_userInfo);
                    await StorageFileHelper.WriteTextToLocalFileAsync(JsonConvert.SerializeObject(_userInfo), UserInfoLocalStorageKey);

                    SetBusyStatus(true, "Redirecting to home screen...");
                    _navigationService.Navigate<HomeViewModel>();
                }
            });

            CancelRegisterUserCommand = new ReactiveCommand();
            CancelRegisterUserCommand.Subscribe(_ =>
            {
                _accountService.SignOut();
                _navigationService.Navigate<HomeViewModel>();
            });
        }

        private async Task LogoffAndRemoveAccount()
        {
            if (HasAccountStored())
            {
                WebAccount account = await GetWebAccount();

                // Check if the account has been deleted already by Token Broker
                if (account != null)
                {
                    if (account.WebAccountProvider.Id == AppSpecificProviderId)
                    {
                        // perform actions to sign out of the app specific provider
                    }
                    else
                    {
                        await account.SignOutAsync();
                    }
                }
                account = null;
                RemoveAccountData();
            }
        }

        private bool HasAccountStored()
        {
            return (ApplicationData.Current.LocalSettings.Values.ContainsKey(StoredAccountIdKey) &&
                    ApplicationData.Current.LocalSettings.Values.ContainsKey(StoredProviderIdKey) &&
                    ApplicationData.Current.LocalSettings.Values.ContainsKey(StoredAuthorityKey));
        }

        private async Task<WebAccount> GetWebAccount()
        {
            String accountID = ApplicationData.Current.LocalSettings.Values[StoredAccountIdKey] as String;
            String providerID = ApplicationData.Current.LocalSettings.Values[StoredProviderIdKey] as String;
            String authority = ApplicationData.Current.LocalSettings.Values[StoredAuthorityKey] as String;

            WebAccount account;

            WebAccountProvider provider = await GetProvider(providerID, authority);

            if (providerID == AppSpecificProviderId)
            {
                account = new WebAccount(provider, accountID, WebAccountState.None);
            }
            else
            {
                account = await WebAuthenticationCoreManager.FindAccountAsync(provider, accountID);

                // The account has been deleted if FindAccountAsync returns null
                if (account == null)
                {
                    RemoveAccountData();
                }
            }

            return account;
        }

        private async Task<WebAccountProvider> GetProvider(string ProviderID, string AuthorityId = "")
        {
            if (ProviderID == AppSpecificProviderId)
            {
                return new WebAccountProvider(AppSpecificProviderId, AppSpecificProviderName, new Uri("/Assets/smallTile-sdk.png"));
            }
            else
            {
                return await WebAuthenticationCoreManager.FindAccountProviderAsync(ProviderID, AuthorityId);
            }
        }

        private static async Task RemoveUserLocalFile()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            if (await localFolder.FileExistsAsync(UserInfoLocalStorageKey))
            {
                var storageFile = await localFolder.GetFileAsync(UserInfoLocalStorageKey);
                await storageFile.DeleteAsync();
            }
        }

        private void RemoveAccountData()
        {
            ApplicationData.Current.LocalSettings.Values.Remove(StoredAccountIdKey);
            ApplicationData.Current.LocalSettings.Values.Remove(StoredProviderIdKey);
            ApplicationData.Current.LocalSettings.Values.Remove(StoredAuthorityKey);
        }


        public void StoreNewAccountDataLocally(UserInfo userInfo)
        {
            if (userInfo.Id != "")
            {
                ApplicationData.Current.LocalSettings.Values[StoredAccountIdKey] = userInfo.Id;
            }
            else
            {
                // It's a custom account
                ApplicationData.Current.LocalSettings.Values[StoredAccountIdKey] = userInfo.Username;
            }


            ApplicationData.Current.LocalSettings.Values[StoredProviderIdKey] = userInfo.ProviderId;
            if (userInfo.Authority != null)
            {
                ApplicationData.Current.LocalSettings.Values[StoredAuthorityKey] = userInfo.Authority;
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values[StoredAuthorityKey] = "";
            }
        }



        private async Task<string> GetJwtToken(string webTokenRequestResultToken)
        {
            return await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    var json = JsonConvert.SerializeObject(new { Username = Username.Value, AccessToken = webTokenRequestResultToken });
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"http://localhost:56375/api/Accounts", stringContent);
                    var responseResult = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<JwtTokenResult>(responseResult);
                    return tokenResponse.Token;
                }
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

        private void SetBusyStatus(bool busy, string busyText)
        {
            IsBusy = busy;
            IsBusyText = busyText;
        }
    }

    public class JwtTokenResult
    {
        public string Token { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class ApplicationUser
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }

}