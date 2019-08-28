using dueltank.Configuration;
using dueltank.ViewModels.Archetypes;
using dueltank.ViewModels.Banlist;
using dueltank.ViewModels.Cards;
using dueltank.ViewModels.Decks;
using dueltank.ViewModels.Home;
using dueltank.ViewModels.Infrastructure.Common;
using dueltank.ViewModels.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;

namespace dueltank.ViewModels.Shell
{

    public class MainShellViewModel : ShellViewModel
    {
        // To obtain Microsoft account tokens, you must register your application online
        // Then, you must associate the app with the store.
        const string MicrosoftAccountProviderId = "https://login.microsoft.com";
        const string ConsumerAuthority = "consumers";
        const string AccountScopeRequested = "wl.basic,wl.emails";
        const string AccountClientId = "none";
        const string StoredAccountKey = "accountid";
        public const string UserInfoLocalStorageKey = "userinfo.json";

        private readonly INavigationService _navigationService;
        private string _hamburgerTitle = "Home";
        private bool _isPaneOpen;
        private object _selectedItem;

        private RelayCommand<NavigationViewSelectionChangedEventArgs> _navigationItemInvoked;
        private RelayCommand<RoutedEventArgs> _backButtonInvoked;
        private RelayCommand<TappedRoutedEventArgs> _signInNavigationItemInvoked;

        private readonly NavigationItem _homeItem = new NavigationItem(0xE80F, "Home", typeof(HomeViewModel));
        private readonly NavigationItem _decksItem = new NavigationItem(0xECAA, "Decks", typeof(DecksViewModel));
        private readonly NavigationItem _banlistItem = new NavigationItem(0xE876, "Banlist", typeof(BanlistViewModel));
        private readonly NavigationItem _cardsItem = new NavigationItem(0xE81E, "Cards", typeof(CardsViewModel));
        private readonly NavigationItem _archetypesItem = new NavigationItem(0xE8EC, "Archetypes", typeof(ArchetypesViewModel));
        private bool _isAuthenticated;
        private UserInfo _userInfo;
        private RelayCommand<TappedRoutedEventArgs> _signOutNavigationItemInvoked;

        public MainShellViewModel()
            : this(ServiceLocator.Current.GetService<INavigationService>())
        {
            AppTitle = "DuelTank";
        }
        public MainShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested += OnAccountCommandsRequested;
        }

        public string AppTitle { get; }

        public IEnumerable<NavigationItem> NavigationViewItems
        {
            get
            {
                yield return _homeItem;
                yield return _decksItem;
                yield return _banlistItem;
                yield return _cardsItem;
                yield return _archetypesItem;
            }
        }


        public string HamburgerTitle
        {
            get => _hamburgerTitle;
            set => Set(ref _hamburgerTitle, value);
        }

        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => Set(ref _isPaneOpen, value);
        }
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set => Set(ref _isAuthenticated, value);
        }

        public object SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        public UserInfo UserInfo
        {
            get => _userInfo ?? UserInfo.Default;
            set => Set(ref _userInfo, value);
        }

        public bool CanGoBack => _navigationService.CanGoBack;

        #region ICommand Members

        public RelayCommand<NavigationViewSelectionChangedEventArgs> NavigationItemInvoked
        {
            get
            {
                return _navigationItemInvoked ?? (_navigationItemInvoked = new RelayCommand<NavigationViewSelectionChangedEventArgs>(OnNavigationItemInvoked, param => true));
            }
            set => _navigationItemInvoked = value;
        }
        public RelayCommand<TappedRoutedEventArgs> SignInNavigationItemInvoked
        {
            get
            {
                return _signInNavigationItemInvoked ?? (_signInNavigationItemInvoked = new RelayCommand<TappedRoutedEventArgs>(OnSignInNavigationItemInvoked, param => true));
            }
            set => _signInNavigationItemInvoked = value;
        }
        public RelayCommand<TappedRoutedEventArgs> SignOutNavigationItemInvoked
        {
            get
            {
                return _signOutNavigationItemInvoked ?? (_signOutNavigationItemInvoked = new RelayCommand<TappedRoutedEventArgs>(OnSignOutNavigationItemInvoked, param => true));
            }
            set => _signOutNavigationItemInvoked = value;
        }

        private void OnSignOutNavigationItemInvoked(TappedRoutedEventArgs obj)
        {
            AccountsSettingsPane.Show();
        }


        public RelayCommand<RoutedEventArgs> BackButtonInvoked
        {
            get
            {
                return _backButtonInvoked ?? (_backButtonInvoked = new RelayCommand<RoutedEventArgs>(OnBackButtonInvoked, param => true));
            }
            set => _backButtonInvoked = value;
        }

        #endregion

        #region ICommand Handlers

        private void OnNavigationItemInvoked(NavigationViewSelectionChangedEventArgs navigationViewItemInvokedEventArgs)
        {
            if (navigationViewItemInvokedEventArgs.SelectedItem is NavigationItem item)
            {
                HamburgerTitle = item.Label;

                NavigateTo(item.ViewModel);
            }
        }
        private void OnBackButtonInvoked(RoutedEventArgs routedEventArgs)
        {
            if (_navigationService.CanGoBack)
            {
                _navigationService.GoBack();
            }
        }

        private void OnSignInNavigationItemInvoked(TappedRoutedEventArgs obj)
        {
            AccountsSettingsPane.Show();
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


        private async void WebAccountProviderCommandInvoked(WebAccountProviderCommand command)
        {
            // ClientID is ignored by MSA
            await RequestTokenAndSaveAccount(command.WebAccountProvider, AccountScopeRequested, AccountClientId);
        }

        private async void WebAccountInvoked(WebAccountCommand command, WebAccountInvokedArgs args)
        {
            if (args.Action == WebAccountAction.Remove)
            {
                await LogoffAndRemoveAccount();
            }
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

                    ApplicationData.Current.LocalSettings.Values[StoredAccountKey] = userAccount.Id;

                    var restApi = new Uri(@"https://apis.live.net/v5.0/me?access_token=" + token);

                    using (var client = new HttpClient())
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
                            AppToken = token,
                        };

                        IRandomAccessStream streamReference = await userAccount.GetPictureAsync(WebAccountPictureSize.Size424x424);
                        if (streamReference != null)
                        {
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.SetSource(streamReference);
                            userInfo.PictureSource = bitmapImage;
                        }

                        //userInfo.PictureSource = $"https://apis.live.net/v5.0/{userAccountInfo.Id}/picture?type=large";

                        await RemoveUserLocalFile();
                        await StorageFileHelper.WriteTextToLocalFileAsync(JsonConvert.SerializeObject(userInfo), UserInfoLocalStorageKey);

                        UserInfo = userInfo;
                        IsAuthenticated = true;
                    }
                }

                OutputTokenResult(webTokenRequestResult);
            }
            catch (Exception ex)
            {
                //rootPage.NotifyUser("Web Token request failed: " + ex.Message, NotifyType.ErrorMessage);
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

                IsAuthenticated = false;
                UserInfo = null;

                //SignInButton.Content = "Sign in";
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

        #endregion

        public void NavigateTo(Type viewModel)
        {
            _navigationService.Navigate(viewModel);
        }

        
    }


    public class MicrosoftUserInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Link { get; set; }
        public object Gender { get; set; }
        public MicrosoftUserEmails Emails { get; set; }
        public string Locale { get; set; }
        public object Updated_Time { get; set; }
    }

    public class MicrosoftUserEmails
    {
        public string Preferred { get; set; }
        public string Account { get; set; }
        public object Personal { get; set; }
        public object Business { get; set; }
    }

    public static class BitmapTools
    {
        public static async Task<BitmapImage> LoadBitmapAsync(byte[] bytes)
        {
            if (bytes != null && bytes.Length > 0)
            {
                using (var stream = new InMemoryRandomAccessStream())
                {
                    var bitmap = new BitmapImage();
                    await stream.WriteAsync(bytes.AsBuffer());
                    stream.Seek(0);
                    await bitmap.SetSourceAsync(stream);
                    return bitmap;
                }
            }
            return null;
        }

        public static async Task<BitmapImage> LoadBitmapAsync(IRandomAccessStream randomAccessStream)
        {
            var bitmap = new BitmapImage();
            try
            {
                using (randomAccessStream)
                {
                    await bitmap.SetSourceAsync(randomAccessStream);
                }
            }
            catch { }
            return bitmap;
        }
    }

}