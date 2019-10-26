using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using dueltank.Configuration;
using dueltank.ViewModels.Accounts;
using dueltank.ViewModels.Home;
using dueltank.ViewModels.Infrastructure;
using dueltank.ViewModels.Infrastructure.Common;
using dueltank.ViewModels.Infrastructure.Services;
using dueltank.ViewModels.Shell;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;

namespace dueltank.Services.Infrastructure
{
    public class AccountService : IAccountService, IDisposable
    {
        private readonly INavigationService _navigationService;
        private readonly IHttpClientFactory _httpClientFactory;

        private const string UserInfoLocalStorageKey = "userinfo.json";
        const string StoredAccountIdKey = "accountid";
        const string StoredProviderIdKey = "providerid";
        const string StoredAuthorityKey = "authority";

        const string AppSpecificProviderId = "dueltankproviderid";
        const string AppSpecificProviderName = "App specific provider";


        const string DefaultProviderId = "https://login.windows.local";
        const string MicrosoftProviderId = "https://login.microsoft.com";
        const string MicrosoftAccountAuthority = "consumers";
        const string AzureActiveDirectoryAuthority = "organizations";

        const string MicrosoftAccountClientId = "none";
        const string MicrosoftAccountScopeRequested = "wl.basic,wl.emails";

        // To obtain azureAD tokens, you must register this app on the AzureAD portal, and obtain the client ID
        const string AzureActiveDirectoryClientId = "";
        const string AzureActiveDirectoryScopeRequested = "";


        public AccountService()
        : this(ServiceLocator.Current.GetService<INavigationService>(), ServiceLocator.Current.GetService<IHttpClientFactory>())
        {

        }
        public AccountService(INavigationService navigationService, IHttpClientFactory httpClientFactory)
        {
            _navigationService = navigationService;
            _httpClientFactory = httpClientFactory;

            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested += OnAccountCommandsRequested;
        }

        public void SignIn()
        {
            AccountsSettingsPane.Show();
        }
        public async Task SignOut()
        {
            await LogoffAndRemoveAccount();
            await RemoveUserLocalFile();
            RemoveAccountData();

            _navigationService.Navigate<HomeViewModel>();
        }

        #region helpers

        public async void OnAccountCommandsRequested(AccountsSettingsPane sender, AccountsSettingsPaneCommandsRequestedEventArgs e)
        {
            // In order to make async calls within this callback, the deferral object is needed
            AccountsSettingsPaneEventDeferral deferral = e.GetDeferral();

            // This scenario only lets the user have one account at a time.
            // If there already is an account, we do not include a provider in the list
            // This will prevent the add account button from showing up.
            if (HasAccountStored())
            {
                await AddWebAccountToPane(e);
            }
            else
            {
                await AddWebAccountProvidersToPane(e);
            }

            AddLinksAndDescription(e);

            deferral.Complete();
        }


        private bool HasAccountStored()
        {
            return (ApplicationData.Current.LocalSettings.Values.ContainsKey(StoredAccountIdKey) &&
                    ApplicationData.Current.LocalSettings.Values.ContainsKey(StoredProviderIdKey) &&
                    ApplicationData.Current.LocalSettings.Values.ContainsKey(StoredAuthorityKey));
        }

        private void RemoveAccountData()
        {
            ApplicationData.Current.LocalSettings.Values.Remove(StoredAccountIdKey);
            ApplicationData.Current.LocalSettings.Values.Remove(StoredProviderIdKey);
            ApplicationData.Current.LocalSettings.Values.Remove(StoredAuthorityKey);
        }


        private async Task AddWebAccountToPane(AccountsSettingsPaneCommandsRequestedEventArgs e)
        {
            WebAccount account = await GetWebAccount();

            WebAccountCommand command = new WebAccountCommand(account, WebAccountInvoked, SupportedWebAccountActions.Remove | SupportedWebAccountActions.Manage);
            e.WebAccountCommands.Add(command);
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

        private async void WebAccountInvoked(WebAccountCommand command, WebAccountInvokedArgs args)
        {
            if (args.Action == WebAccountAction.Remove)
            {
                //rootPage.NotifyUser("Removing account", NotifyType.StatusMessage);
                await LogoffAndRemoveAccount();
            }
            else if (args.Action == WebAccountAction.Manage)
            {
                // Display user management UI for this account
                //rootPage.NotifyUser("Managing account", NotifyType.StatusMessage);
            }
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

        private async Task AddWebAccountProvidersToPane(AccountsSettingsPaneCommandsRequestedEventArgs e)
        {
            List<WebAccountProvider> providers = new List<WebAccountProvider>();

            // Microsoft account and Azure AD providers will always return. Non-installed plugins or incorrect identities will return null
            providers.Add(await GetProvider(MicrosoftProviderId, MicrosoftAccountAuthority));
            //providers.Add(await GetProvider(MicrosoftProviderId, AzureActiveDirectoryAuthority));
            //providers.Add(await GetProvider(AppSpecificProviderId));

            foreach (WebAccountProvider provider in providers)
            {
                WebAccountProviderCommand providerCommand = new WebAccountProviderCommand(provider, WebAccountProviderCommandInvoked);
                e.WebAccountProviderCommands.Add(providerCommand);
            }
        }

        private void WebAccountProviderCommandInvoked(WebAccountProviderCommand command)
        {
            if ((command.WebAccountProvider.Id == MicrosoftProviderId) && (command.WebAccountProvider.Authority == MicrosoftAccountAuthority))
            {
                // ClientID is ignored by MSA
                AuthenticateWithRequestToken(command.WebAccountProvider, MicrosoftAccountScopeRequested, MicrosoftAccountClientId);
            }
            else if ((command.WebAccountProvider.Id == MicrosoftProviderId) && (command.WebAccountProvider.Authority == AzureActiveDirectoryAuthority))
            {
                AuthenticateWithRequestToken(command.WebAccountProvider, AzureActiveDirectoryAuthority, AzureActiveDirectoryClientId);
            }
            else if (command.WebAccountProvider.Id == AppSpecificProviderId)
            {
                // Show user registration/login for your app specific account type.
                // Store the user if registration/login successful
                StoreNewAccountDataLocally(new WebAccount(command.WebAccountProvider, "App Specific User", WebAccountState.None));
            }
        }

        public async void AuthenticateWithRequestToken(WebAccountProvider Provider, String Scope, String ClientID)
        {
            try
            {
                WebTokenRequest webTokenRequest = new WebTokenRequest(Provider, Scope, ClientID);

                // Azure Active Directory requires a resource to return a token
                if (Provider.Id == "https://login.microsoft.com" && Provider.Authority == "organizations")
                {
                    webTokenRequest.Properties.Add("resource", "https://graph.windows.net");
                }

                // If the user selected a specific account, RequestTokenAsync will return a token for that account.
                // The user may be prompted for credentials or to authorize using that account with your app
                // If the user selected a provider, the user will be prompted for credentials to login to a new account
                WebTokenRequestResult webTokenRequestResult = await WebAuthenticationCoreManager.RequestTokenAsync(webTokenRequest);

                if (webTokenRequestResult.ResponseStatus == WebTokenRequestStatus.Success)
                {
                    //StoreNewAccountDataLocally(webTokenRequestResult.ResponseData[0].WebAccount);

                    var userAccount = webTokenRequestResult.ResponseData[0].WebAccount;
                    var token = webTokenRequestResult.ResponseData[0].Token;
                    var properties = webTokenRequestResult.ResponseData[0].Properties;

                    var restApi = new Uri(@"https://apis.live.net/v5.0/me?access_token=" + token);
                    var dueltankTokenApi = new Uri(@"http://localhost:56375/api/Accounts/MicrosoftLogin?token=" + token);

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
                            Email = userAccountInfo.Emails.Preferred ?? userAccountInfo.Emails.Account,
                            ProviderId = userAccount.WebAccountProvider.Id,
                            Authority = userAccount.WebAccountProvider.Authority,
                            WebTokenRequestResultToken = token
                        };

                        var dueltankTokenResult = await client.GetAsync(dueltankTokenApi);

                        if(dueltankTokenResult.IsSuccessStatusCode)
                        {
                            string tokenContent = await dueltankTokenResult.Content.ReadAsStringAsync();
                            var jwtTokenResult = JsonConvert.DeserializeObject<JwtTokenResult>(tokenContent);

                            var jwtToken = jwtTokenResult.Token;
                            userInfo.JwtToken = jwtToken;

                            await SignOut();

                            StoreNewAccountDataLocally(webTokenRequestResult.ResponseData[0].WebAccount);
                            await StorageFileHelper.WriteTextToLocalFileAsync(JsonConvert.SerializeObject(userInfo), UserInfoLocalStorageKey);

                            _navigationService.Navigate<HomeViewModel>();

                        }
                        else
                        {
                            _navigationService.Navigate<UsernameViewModel>(userInfo);
                        }
                    }
                }

                OutputTokenResult(webTokenRequestResult);
            }
            catch (Exception ex)
            {
                //rootPage.NotifyUser("Web Token request failed: " + ex, NotifyType.ErrorMessage);
            }
        }

        public void StoreNewAccountDataLocally(WebAccount account)
        {
            if (account.Id != "")
            {
                ApplicationData.Current.LocalSettings.Values["AccountID"] = account.Id;
            }
            else
            {
                // It's a custom account
                ApplicationData.Current.LocalSettings.Values["AccountID"] = account.UserName;
            }


            ApplicationData.Current.LocalSettings.Values["ProviderID"] = account.WebAccountProvider.Id;
            if (account.WebAccountProvider.Authority != null)
            {
                ApplicationData.Current.LocalSettings.Values["Authority"] = account.WebAccountProvider.Authority;
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values["Authority"] = "";
            }
        }

        public void OutputTokenResult(WebTokenRequestResult result)
        {
            if (result.ResponseStatus == WebTokenRequestStatus.Success)
            {
                //rootPage.NotifyUser("Web Token request successful for user:" + result.ResponseData[0].WebAccount.UserName, NotifyType.StatusMessage);
            }
            else
            {
                //rootPage.NotifyUser("Web Token request error: " + result.ResponseStatus + " Code: " + result.ResponseError.ErrorMessage, NotifyType.StatusMessage);
            }
        }

        private void AddLinksAndDescription(AccountsSettingsPaneCommandsRequestedEventArgs e)
        {
            e.HeaderText = "Signing in lets you sync your decks across devices.";

            // You can add links such as privacy policy, help, general account settings
            //e.Commands.Add(new SettingsCommand("privacypolicy", "Privacy policy", PrivacyPolicyInvoked));
            //e.Commands.Add(new SettingsCommand("otherlink", "Other link", OtherLinkInvoked));
        }

        private void OtherLinkInvoked(IUICommand command)
        {
            //rootPage.NotifyUser("Other link pressed by user", NotifyType.StatusMessage);
        }

        private void PrivacyPolicyInvoked(IUICommand command)
        {
            //rootPage.NotifyUser("Privacy policy clicked by user", NotifyType.StatusMessage);
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



        #endregion

        public void Dispose()
        {
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested -= OnAccountCommandsRequested;
        }
    }
}