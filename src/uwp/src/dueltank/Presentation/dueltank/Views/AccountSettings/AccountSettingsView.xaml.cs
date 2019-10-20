using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using dueltank.ViewModels.Accounts;
using dueltank.ViewModels.Infrastructure.Common;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace dueltank.Views.AccountSettings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccountSettingsView : Page
    {
        public AccountSettingsView()
        {
            this.InitializeComponent();
            this.Loaded += (sender, args) =>
            {
                var viewModel = DataContext as AccountSettingsViewModel;
                viewModel.ShowAccountSettingsDialog();
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            var viewModel = DataContext as AccountSettingsViewModel;
            //viewModel?.LoadUserInfo(e.Parameter as UserInfo);

            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested += (DataContext as AccountSettingsViewModel).OnAccountCommandsRequested;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested -= (DataContext as AccountSettingsViewModel).OnAccountCommandsRequested;

            base.OnNavigatingFrom(e);
        }
    }
}
