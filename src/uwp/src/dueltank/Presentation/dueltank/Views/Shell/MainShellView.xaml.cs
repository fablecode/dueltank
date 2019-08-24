using dueltank.Configuration;
using dueltank.Services.Infrastructure;
using dueltank.ViewModels.Home;
using dueltank.ViewModels.Infrastructure.Services;
using dueltank.ViewModels.Shell;
using System;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using dueltank.ViewModels.Infrastructure.Common;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace dueltank.Views.Shell
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainShellView : Page
    {
        private INavigationService _navigationService;

        public MainShellView()
        {
            InitializeComponent();
            InitializeNavigation();
        }

        private static SystemNavigationManager CurrentView => SystemNavigationManager.GetForCurrentView();


        #region private helpers

        private void InitializeNavigation()
        {
            _navigationService = ServiceLocator.Current.GetService<INavigationService>();
            _navigationService.Initialize(ContentFrame);
            CurrentView.BackRequested += OnBackRequested;
            ContentFrame.Navigated += OnFrameNavigated;
        }

        private void UpdateBackButton()
        {
            if (_navigationService.CanGoBack)
            {
                NavigationBackButton.Visibility = Visibility.Visible;
                AppTitleTextBlock.Padding = new Thickness(40,0,0,0);
            }
            else
            {
                NavigationBackButton.Visibility = Visibility.Collapsed;
                AppTitleTextBlock.Padding = new Thickness(10, 0, 0, 0);
            }
        }

        private void CustomizeTitleBar()
        {
            // Hide default title bar.
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            Window.Current.SetTitleBar(trickyTitleBar);

            // customize buttons' colors
            //AppTitleBar.Height = coreTitleBar.Height;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.BackgroundColor = Colors.Red;
            titleBar.ForegroundColor = Colors.White;

            titleBar.ButtonBackgroundColor = Colors.Red;
            titleBar.ButtonHoverBackgroundColor = GetSolidColorBrush("#40FFFFFF ").Color;
            titleBar.ButtonHoverForegroundColor = Colors.White;
            titleBar.ButtonInactiveForegroundColor = GetSolidColorBrush("#FFF58989").Color;
            titleBar.ButtonInactiveBackgroundColor = Colors.Red;
        } 

        #endregion

        public SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            var a = (byte) Convert.ToUInt32(hex.Substring(0, 2), 16);
            var r = (byte) Convert.ToUInt32(hex.Substring(2, 2), 16);
            var g = (byte) Convert.ToUInt32(hex.Substring(4, 2), 16);
            var b = (byte) Convert.ToUInt32(hex.Substring(6, 2), 16);
            var myBrush = new SolidColorBrush(Color.FromArgb(a, r, g, b));
            return myBrush;
        }

        #region Overridden members

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            CustomizeTitleBar();

            var viewModel = (MainShellViewModel)DataContext;
            var localFolder = ApplicationData.Current.LocalFolder;

            if (await localFolder.FileExistsAsync(MainShellViewModel.UserInfoLocalStorageKey))
            {
                viewModel.UserInfo = JsonConvert.DeserializeObject<UserInfo>(await StorageFileHelper.ReadTextFromLocalFileAsync(MainShellViewModel.UserInfoLocalStorageKey));
                viewModel.IsAuthenticated = viewModel.UserInfo != null;
            }

            viewModel.NavigateTo(typeof(HomeViewModel));
        }

        #endregion

        #region Event handlers

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (_navigationService.CanGoBack)
            {
                _navigationService.GoBack();
            }
        }

        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            var targetType = NavigationService.GetViewModel(e.SourcePageType);
            var viewModel = (MainShellViewModel)DataContext;

            switch (targetType.Name)
            {
                case "SettingsViewModel":
                    viewModel.SelectedItem = navigationView.SettingsItem;
                    break;
                default:
                    viewModel.SelectedItem = viewModel.NavigationViewItems.FirstOrDefault(r => r.ViewModel == targetType);
                    break;
            }
            UpdateBackButton();
        }

        #endregion
    }
}