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
using System.Net.Http;
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
using dueltank.ViewModels.Accounts;
using dueltank.ViewModels.Infrastructure;
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
        private readonly IAccountService _accountService;
        private readonly IHttpClientFactory _httpClientFactory;
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
        private bool _isBusy= true;

        public MainShellViewModel()
            : this(ServiceLocator.Current.GetService<INavigationService>(), ServiceLocator.Current.GetService<IAccountService>(), ServiceLocator.Current.GetService<IHttpClientFactory>())
        {
            AppTitle = "DuelTank";
        }
        public MainShellViewModel(INavigationService navigationService, IAccountService accountService, IHttpClientFactory httpClientFactory)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            _httpClientFactory = httpClientFactory;
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
        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
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
            _accountService.SignOut().GetAwaiter();
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

                _navigationService.Navigate(item.ViewModel);
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
            NavigateTo(typeof(AccountSettingsViewModel));
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