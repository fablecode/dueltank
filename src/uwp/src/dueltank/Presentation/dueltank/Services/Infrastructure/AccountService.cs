using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.ApplicationSettings;
using dueltank.Configuration;
using dueltank.ViewModels.Accounts;
using dueltank.ViewModels.Infrastructure;
using dueltank.ViewModels.Infrastructure.Common;
using dueltank.ViewModels.Infrastructure.Services;
using dueltank.ViewModels.Shell;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;

namespace dueltank.Services.Infrastructure
{
    public class AccountService : IAccountService
    {
        private readonly INavigationService _navigationService;
        private readonly IHttpClientFactory _httpClientFactory;

        // To obtain Microsoft account tokens, you must register your application online
        // Then, you must associate the app with the store.
        private const string MicrosoftAccountProviderId = "https://login.microsoft.com";
        private const string ConsumerAuthority = "consumers";
        private const string AccountScopeRequested = "wl.basic,wl.emails";
        private const string AccountClientId = "none";
        private const string StoredAccountKey = "accountid";
        private const string UserInfoLocalStorageKey = "userinfo.json";

        public AccountService()
        : this(ServiceLocator.Current.GetService<INavigationService>(), ServiceLocator.Current.GetService<IHttpClientFactory>())
        {

        }
        public AccountService(INavigationService navigationService, IHttpClientFactory httpClientFactory)
        {
            _navigationService = navigationService;
            _httpClientFactory = httpClientFactory;

            //AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested += OnAccountCommandsRequested;
        }

        public void SignIn()
        {
            AccountsSettingsPane.Show();
        }
        public Task SignOut()
        {
            return LogoffAndRemoveAccount();
        }


        // This event handler is called when the Account settings pane is to be launched.
        private async void OnAccountCommandsRequested(AccountsSettingsPane sender, AccountsSettingsPaneCommandsRequestedEventArgs e)
        {
            // In order to make async calls within this callback, the deferral object is needed
            AccountsSettingsPaneEventDeferral deferral = e.GetDeferral();

            // This scenario only lets the user have one account at a time.
            // If there already is an account, we do not include a provider in the list
            // This will prevent the add account button from showing up.
            bool isPresent = ApplicationData.Current.LocalSettings.Values.ContainsKey(StoredAccountKey);

            if (isPresent)
            {
                await AddWebAccount(e);
            }
            else
            {
                await AddWebAccountProvider(e);
            }

            deferral.Complete();
        }

        private async Task AddWebAccount(AccountsSettingsPaneCommandsRequestedEventArgs e)
        {
            WebAccountProvider provider = await WebAuthenticationCoreManager.FindAccountProviderAsync(MicrosoftAccountProviderId, ConsumerAuthority);

            String accountID = (String)ApplicationData.Current.LocalSettings.Values[StoredAccountKey];
            WebAccount account = await WebAuthenticationCoreManager.FindAccountAsync(provider, accountID);

            if (account == null)
            {
                // The account has most likely been deleted in Windows settings
                // Unless there would be significant data loss, you should just delete the account
                // If there would be significant data loss, prompt the user to either re-add the account, or to remove it
                RemoveUserLocalSettings();
            }

            WebAccountCommand command = new WebAccountCommand(account, WebAccountInvoked, SupportedWebAccountActions.Remove);
            e.WebAccountCommands.Add(command);
        }

        private async Task AddWebAccountProvider(AccountsSettingsPaneCommandsRequestedEventArgs e)
        {
            // FindAccountProviderAsync returns the WebAccountProvider of an installed plugin
            // The Provider and Authority specifies the specific plugin
            // This scenario only supports Microsoft accounts.

            // The Microsoft account provider is always present in Windows 10 devices, as is the Azure AD plugin.
            // If a non-installed plugin or incorect identity is specified, FindAccountProviderAsync will return null   
            WebAccountProvider msaProvider = await WebAuthenticationCoreManager.FindAccountProviderAsync(MicrosoftAccountProviderId, ConsumerAuthority);

            WebAccountProviderCommand msaProviderCommand = new WebAccountProviderCommand(msaProvider, WebAccountProviderCommandInvoked);

            e.WebAccountProviderCommands.Add(msaProviderCommand);
        }

        private async Task RequestTokenAndSaveAccount(WebAccountProvider Provider, String Scope, String ClientID)
        {
            try
            {
                WebTokenRequest webTokenRequest = new WebTokenRequest(Provider, Scope, ClientID);
                //rootPage.NotifyUser("Requesting Web Token", NotifyType.StatusMessage);

                // If the user selected a specific account, RequestTokenAsync will return a token for that account.
                // The user may be prompted for credentials or to authorize using that account with your app
                // If the user selected a provider, the user will be prompted for credentials to login to a new account
                WebTokenRequestResult webTokenRequestResult = await WebAuthenticationCoreManager.RequestTokenAsync(webTokenRequest);

                // If a token was successfully returned, then store the WebAccount Id into local app data
                // This Id can be used to retrieve the account whenever needed. To later get a token with that account
                // First retrieve the account with FindAccountAsync, and include that webaccount 
                // as a parameter to RequestTokenAsync or RequestTokenSilentlyAsync
                if (webTokenRequestResult.ResponseStatus == WebTokenRequestStatus.Success)
                {
                    RemoveUserLocalSettings();

                    var userAccount = webTokenRequestResult.ResponseData[0].WebAccount;
                    var token = webTokenRequestResult.ResponseData[0].Token;
                    var properties = webTokenRequestResult.ResponseData[0].Properties;

                    ApplicationData.Current.LocalSettings.Values[StoredAccountKey] = userAccount.Id;

                    var restApi = new Uri(@"https://apis.live.net/v5.0/me?access_token=" + token);

                    using (var client = _httpClientFactory.CreateClient())
                    {
                        var infoResult = await client.GetAsync(restApi);
                        string content = await infoResult.Content.ReadAsStringAsync();
                        var userAccountInfo = JsonConvert.DeserializeObject<MicrosoftUserInfo>(content);


                        var userInfo = new UserInfo
                        {
                            Id = userAccountInfo.Id,
                            FirstName = userAccountInfo.First_Name,
                            LastName = userAccountInfo.Last_Name,
                            Email = userAccountInfo.Emails.Preferred ?? userAccountInfo.Emails.Account
                        };

                        //var usernameDialog = new UsernameContentDialog(userInfo);

                        //var dialogResult = await usernameDialog.ShowAsync();

                        _navigationService.Navigate<UsernameViewModel>(userInfo);

                        //IRandomAccessStream streamReference = await userAccount.GetPictureAsync(WebAccountPictureSize.Size424x424);
                        //if (streamReference != null)
                        //{
                        //    var bitmapImage = new BitmapImage();
                        //    bitmapImage.SetSource(streamReference);
                        //    userInfo.PictureSource = bitmapImage;
                        //}

                        ////userInfo.PictureSource = $"https://apis.live.net/v5.0/{userAccountInfo.Id}/picture?type=large";

                        //await RemoveUserLocalFile();
                        //await StorageFileHelper.WriteTextToLocalFileAsync(JsonConvert.SerializeObject(userInfo), UserInfoLocalStorageKey);

                        //UserInfo = userInfo;
                        //IsAuthenticated = true;
                    }
                }

                OutputTokenResult(webTokenRequestResult);
            }
            catch (Exception ex)
            {
                //rootPage.NotifyUser("Web Token request failed: " + ex.Message, NotifyType.ErrorMessage);
            }
        }

        private async void WebAccountInvoked(WebAccountCommand command, WebAccountInvokedArgs args)
        {
            if (args.Action == WebAccountAction.Remove)
            {
                await LogoffAndRemoveAccount();
            }
        }

        private async void WebAccountProviderCommandInvoked(WebAccountProviderCommand command)
        {
            // ClientID is ignored by MSA
            await RequestTokenAndSaveAccount(command.WebAccountProvider, AccountScopeRequested, AccountClientId);
        }


        private async Task LogoffAndRemoveAccount()
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(StoredAccountKey))
            {
                WebAccountProvider providertoDelete = await WebAuthenticationCoreManager.FindAccountProviderAsync(MicrosoftAccountProviderId, ConsumerAuthority);

                WebAccount accountToDelete = await WebAuthenticationCoreManager.FindAccountAsync(providertoDelete, (string)ApplicationData.Current.LocalSettings.Values[StoredAccountKey]);

                if (accountToDelete != null)
                {
                    await accountToDelete.SignOutAsync();
                }

                RemoveUserLocalSettings();
                await RemoveUserLocalFile();

                //IsAuthenticated = false;
                //UserInfo = null;

                //SignInButton.Content = "Sign in";
            }
        }

        private void OutputTokenResult(WebTokenRequestResult result)
        {
            if (result.ResponseStatus == WebTokenRequestStatus.Success)
            {
                //rootPage.NotifyUser("Web Token request successful for user: " + result.ResponseData[0].WebAccount.UserName, NotifyType.StatusMessage);
                //SignInButton.Content = "Account";
            }
            else
            {
                //rootPage.NotifyUser("Web Token request error: " + result.ResponseError, NotifyType.StatusMessage);
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

        private static void RemoveUserLocalSettings()
        {
            ApplicationData.Current.LocalSettings.Values.Remove(StoredAccountKey);
        }
    }
}