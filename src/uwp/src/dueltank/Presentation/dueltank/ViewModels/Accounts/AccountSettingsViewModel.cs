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
using dueltank.ViewModels.Infrastructure;
using dueltank.ViewModels.Infrastructure.Common;
using dueltank.ViewModels.Infrastructure.Services;
using dueltank.ViewModels.Infrastructure.ViewModels;
using dueltank.ViewModels.Shell;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;

namespace dueltank.ViewModels.Accounts
{
    public class AccountSettingsViewModel : ViewModelBase
    {
        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;
        private readonly IHttpClientFactory _httpClientFactory;
        private bool _isBusy;

        // To obtain Microsoft account tokens, you must register your application online
        // Then, you must associate the app with the store.
        private const string MicrosoftAccountProviderId = "https://login.microsoft.com";
        private const string ConsumerAuthority = "consumers";
        private const string AccountScopeRequested = "wl.basic,wl.emails";
        private const string AccountClientId = "none";
        private const string StoredAccountKey = "accountid";
        private const string UserInfoLocalStorageKey = "userinfo.json";

        const string DefaultProviderId = "https://login.windows.local";
        const string MicrosoftProviderId = "https://login.microsoft.com";
        const string MicrosoftAccountAuthority = "consumers";
        const string AzureActiveDirectoryAuthority = "organizations";

        const string MicrosoftAccountClientId = "none";
        const string MicrosoftAccountScopeRequested = "wl.basic,wl.emails";

        WebAccountProvider defaultProvider;
        WebAccount defaultAccount;


        const string StoredAccountIdKey = "accountid";
        const string StoredProviderIdKey = "providerid";
        const string StoredAuthorityKey = "authority";

        const string AppSpecificProviderId = "dueltankproviderid";
        const string AppSpecificProviderName = "App specific provider";

        // To obtain azureAD tokens, you must register this app on the AzureAD portal, and obtain the client ID
        const string AzureActiveDirectoryClientId = "";
        const string AzureActiveDirectoryScopeRequested = "";


        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        public AccountSettingsViewModel()
            : this(ServiceLocator.Current.GetService<IAccountService>(), ServiceLocator.Current.GetService<INavigationService>(), ServiceLocator.Current.GetService<IHttpClientFactory>())
        {

        }

        public AccountSettingsViewModel(IAccountService accountService, INavigationService navigationService, IHttpClientFactory httpClientFactory)
        {
            _accountService = accountService;
            _navigationService = navigationService;
            _httpClientFactory = httpClientFactory;

        }

        public void ShowAccountSettingsDialog()
        {
            _accountService.SignIn();
            //AccountsSettingsPane.Show();


            //defaultProvider = await WebAuthenticationCoreManager.FindAccountProviderAsync(DefaultProviderId);

            //if (defaultProvider == null)
            //{
            //    //rootPage.NotifyUser("This user does not have a primary account", NotifyType.StatusMessage);
            //}
            //else
            //{
            //    // Set the clientID and scope based on the authority. The authority tells us if the account is a Microsoft account or an Azure AD.
            //    String scope = MicrosoftAccountScopeRequested;
            //    String clientID = MicrosoftAccountClientId;

            //    AuthenticateWithRequestToken(defaultProvider, scope, clientID);
            //}

        }

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

                        _navigationService.Navigate<UsernameViewModel>(userInfo);
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
            e.HeaderText = "Signing in lets you sync you decks across devices.";

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














        //public async void AuthenticateWithRequestToken(WebAccountProvider Provider, String Scope, String ClientID)
        //{
        //    try
        //    {
        //        WebTokenRequest webTokenRequest = new WebTokenRequest(Provider, Scope, ClientID);

        //        // Azure Active Directory requires a resource to return a token
        //        if (Provider.Id == "https://login.microsoft.com" && Provider.Authority == "organizations")
        //        {
        //            webTokenRequest.Properties.Add("resource", "https://graph.windows.net");
        //        }

        //        // If the user selected a specific account, RequestTokenAsync will return a token for that account.
        //        // The user may be prompted for credentials or to authorize using that account with your app
        //        // If the user selected a provider, the user will be prompted for credentials to login to a new account
        //        WebTokenRequestResult webTokenRequestResult = await WebAuthenticationCoreManager.RequestTokenAsync(webTokenRequest);

        //        if (webTokenRequestResult.ResponseStatus == WebTokenRequestStatus.Success)
        //        {
        //            defaultAccount = webTokenRequestResult.ResponseData[0].WebAccount;

        //            var userAccount = webTokenRequestResult.ResponseData[0].WebAccount;
        //            var token = webTokenRequestResult.ResponseData[0].Token;

        //            var properties = webTokenRequestResult.ResponseData[0].Properties;

        //            ApplicationData.Current.LocalSettings.Values[StoredAccountKey] = userAccount.Id;

        //            var restApi = new Uri(@"https://apis.live.net/v5.0/me?access_token=" + token);

        //            using (var client = _httpClientFactory.CreateClient())
        //            {
        //                var infoResult = await client.GetAsync(restApi);
        //                string content = await infoResult.Content.ReadAsStringAsync();
        //                var userAccountInfo = JsonConvert.DeserializeObject<MicrosoftUserInfo>(content);

        //                var userInfo = new UserInfo
        //                {
        //                    Id = userAccountInfo.Id,
        //                    FirstName = userAccountInfo.First_Name,
        //                    LastName = userAccountInfo.Last_Name,
        //                    Email = userAccountInfo.Emails.Preferred ?? userAccountInfo.Emails.Account
        //                };
        //            }


        //            //button_GetTokenSilently.IsEnabled = true;
        //            //textblock_SignedInStatus.Text = "Signed in with:";
        //            //textblock_SignedInStatus.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
        //            //listview_SignedInAccounts.Items.Clear();
        //            //listview_SignedInAccounts.Items.Add(defaultAccount.Id);
        //        }

        //        OutputTokenResult(webTokenRequestResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        //rootPage.NotifyUser("Web Token request failed: " + ex, NotifyType.ErrorMessage);
        //    }
        //}

        //public void OutputTokenResult(WebTokenRequestResult result)
        //{
        //    if (result.ResponseStatus == WebTokenRequestStatus.Success)
        //    {
        //        //rootPage.NotifyUser("Web Token request successful for user:" + result.ResponseData[0].WebAccount.UserName, NotifyType.StatusMessage);
        //    }
        //    else
        //    {
        //        //rootPage.NotifyUser("Web Token request error: " + result.ResponseStatus + " Code: " + result.ResponseError.ErrorMessage, NotifyType.StatusMessage);
        //    }
        //}








        //// This event handler is called when the Account settings pane is to be launched.
        //private async void OnAccountCommandsRequested(AccountsSettingsPane sender, AccountsSettingsPaneCommandsRequestedEventArgs e)
        //{
        //    IsBusy = true;

        //    // In order to make async calls within this callback, the deferral object is needed
        //    AccountsSettingsPaneEventDeferral deferral = e.GetDeferral();

        //    // This scenario only lets the user have one account at a time.
        //    // If there already is an account, we do not include a provider in the list
        //    // This will prevent the add account button from showing up.
        //    bool isPresent = ApplicationData.Current.LocalSettings.Values.ContainsKey(StoredAccountKey);

        //    if (isPresent)
        //    {
        //        await AddWebAccount(e);
        //    }
        //    else
        //    {
        //        await AddWebAccountProvider(e);
        //    }

        //    deferral.Complete();
        //}

        //private bool IsSignedIn()
        //{
        //    return ApplicationData.Current.LocalSettings.Values.ContainsKey(StoredAccountKey);
        //}

        //private async Task AddWebAccount(AccountsSettingsPaneCommandsRequestedEventArgs e)
        //{
        //    WebAccountProvider provider = await WebAuthenticationCoreManager.FindAccountProviderAsync(MicrosoftAccountProviderId, ConsumerAuthority);

        //    String accountID = (String)ApplicationData.Current.LocalSettings.Values[StoredAccountKey];
        //    WebAccount account = await WebAuthenticationCoreManager.FindAccountAsync(provider, accountID);

        //    if (account == null)
        //    {
        //        // The account has most likely been deleted in Windows settings
        //        // Unless there would be significant data loss, you should just delete the account
        //        // If there would be significant data loss, prompt the user to either re-add the account, or to remove it
        //        RemoveUserLocalSettings();
        //    }

        //    WebAccountCommand command = new WebAccountCommand(account, WebAccountInvoked, SupportedWebAccountActions.Remove);
        //    e.WebAccountCommands.Add(command);
        //}

        //private async Task AddWebAccountProvider(AccountsSettingsPaneCommandsRequestedEventArgs e)
        //{
        //    // FindAccountProviderAsync returns the WebAccountProvider of an installed plugin
        //    // The Provider and Authority specifies the specific plugin
        //    // This scenario only supports Microsoft accounts.

        //    // The Microsoft account provider is always present in Windows 10 devices, as is the Azure AD plugin.
        //    // If a non-installed plugin or incorect identity is specified, FindAccountProviderAsync will return null   
        //    WebAccountProvider msaProvider = await WebAuthenticationCoreManager.FindAccountProviderAsync(MicrosoftAccountProviderId, ConsumerAuthority);

        //    WebAccountProviderCommand msaProviderCommand = new WebAccountProviderCommand(msaProvider, WebAccountProviderCommandInvoked);

        //    e.WebAccountProviderCommands.Add(msaProviderCommand);
        //}

        //private async Task RequestTokenAndSaveAccount(WebAccountProvider Provider, String Scope, String ClientID)
        //{
        //    try
        //    {
        //        WebTokenRequest webTokenRequest = new WebTokenRequest(Provider, Scope, ClientID);
        //        //rootPage.NotifyUser("Requesting Web Token", NotifyType.StatusMessage);

        //        // If the user selected a specific account, RequestTokenAsync will return a token for that account.
        //        // The user may be prompted for credentials or to authorize using that account with your app
        //        // If the user selected a provider, the user will be prompted for credentials to login to a new account
        //        WebTokenRequestResult webTokenRequestResult = await WebAuthenticationCoreManager.RequestTokenAsync(webTokenRequest);

        //        // If a token was successfully returned, then store the WebAccount Id into local app data
        //        // This Id can be used to retrieve the account whenever needed. To later get a token with that account
        //        // First retrieve the account with FindAccountAsync, and include that webaccount 
        //        // as a parameter to RequestTokenAsync or RequestTokenSilentlyAsync
        //        if (webTokenRequestResult.ResponseStatus == WebTokenRequestStatus.Success)
        //        {
        //            RemoveUserLocalSettings();

        //            var userAccount = webTokenRequestResult.ResponseData[0].WebAccount;
        //            var token = webTokenRequestResult.ResponseData[0].Token;
        //            var properties = webTokenRequestResult.ResponseData[0].Properties;

        //            ApplicationData.Current.LocalSettings.Values[StoredAccountKey] = userAccount.Id;

        //            var restApi = new Uri(@"https://apis.live.net/v5.0/me?access_token=" + token);

        //            using (var client = _httpClientFactory.CreateClient())
        //            {
        //                var infoResult = await client.GetAsync(restApi);
        //                string content = await infoResult.Content.ReadAsStringAsync();
        //                var userAccountInfo = JsonConvert.DeserializeObject<MicrosoftUserInfo>(content);


        //                var userInfo = new UserInfo
        //                {
        //                    Id = userAccountInfo.Id,
        //                    FirstName = userAccountInfo.First_Name,
        //                    LastName = userAccountInfo.Last_Name,
        //                    Email = userAccountInfo.Emails.Preferred ?? userAccountInfo.Emails.Account
        //                };

        //                //var usernameDialog = new UsernameContentDialog(userInfo);

        //                //var dialogResult = await usernameDialog.ShowAsync();

        //                _navigationService.Navigate<UsernameViewModel>(userInfo);

        //                //IRandomAccessStream streamReference = await userAccount.GetPictureAsync(WebAccountPictureSize.Size424x424);
        //                //if (streamReference != null)
        //                //{
        //                //    var bitmapImage = new BitmapImage();
        //                //    bitmapImage.SetSource(streamReference);
        //                //    userInfo.PictureSource = bitmapImage;
        //                //}

        //                ////userInfo.PictureSource = $"https://apis.live.net/v5.0/{userAccountInfo.Id}/picture?type=large";

        //                //await RemoveUserLocalFile();
        //                //await StorageFileHelper.WriteTextToLocalFileAsync(JsonConvert.SerializeObject(userInfo), UserInfoLocalStorageKey);

        //                //UserInfo = userInfo;
        //                //IsAuthenticated = true;
        //                IsBusy = false;
        //            }
        //        }

        //        OutputTokenResult(webTokenRequestResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        //rootPage.NotifyUser("Web Token request failed: " + ex.Message, NotifyType.ErrorMessage);
        //    }
        //}

        //private async void WebAccountInvoked(WebAccountCommand command, WebAccountInvokedArgs args)
        //{
        //    if (args.Action == WebAccountAction.Remove)
        //    {
        //        await LogoffAndRemoveAccount();
        //    }
        //}

        //private async void WebAccountProviderCommandInvoked(WebAccountProviderCommand command)
        //{
        //    // ClientID is ignored by MSA
        //    await RequestTokenAndSaveAccount(command.WebAccountProvider, AccountScopeRequested, AccountClientId);
        //}


        //private async Task LogoffAndRemoveAccount()
        //{
        //    if (ApplicationData.Current.LocalSettings.Values.ContainsKey(StoredAccountKey))
        //    {
        //        WebAccountProvider providertoDelete = await WebAuthenticationCoreManager.FindAccountProviderAsync(MicrosoftAccountProviderId, ConsumerAuthority);

        //        WebAccount accountToDelete = await WebAuthenticationCoreManager.FindAccountAsync(providertoDelete, (string)ApplicationData.Current.LocalSettings.Values[StoredAccountKey]);

        //        if (accountToDelete != null)
        //        {
        //            await accountToDelete.SignOutAsync();
        //        }

        //        RemoveUserLocalSettings();
        //        await RemoveUserLocalFile();

        //        //IsAuthenticated = false;
        //        //UserInfo = null;

        //        //SignInButton.Content = "Sign in";
        //    }
        //}

        //private void OutputTokenResult(WebTokenRequestResult result)
        //{
        //    if (result.ResponseStatus == WebTokenRequestStatus.Success)
        //    {
        //        //rootPage.NotifyUser("Web Token request successful for user: " + result.ResponseData[0].WebAccount.UserName, NotifyType.StatusMessage);
        //        //SignInButton.Content = "Account";
        //    }
        //    else
        //    {
        //        //rootPage.NotifyUser("Web Token request error: " + result.ResponseError, NotifyType.StatusMessage);
        //    }
        //}


        //private static async Task RemoveUserLocalFile()
        //{
        //    var localFolder = ApplicationData.Current.LocalFolder;
        //    if (await localFolder.FileExistsAsync(UserInfoLocalStorageKey))
        //    {
        //        var storageFile = await localFolder.GetFileAsync(UserInfoLocalStorageKey);
        //        await storageFile.DeleteAsync();
        //    }
        //}

        //private static void RemoveUserLocalSettings()
        //{
        //    ApplicationData.Current.LocalSettings.Values.Remove(StoredAccountKey);
        //}

    }
}