using dueltank.Helpers;
using Windows.UI.Xaml.Controls;

namespace dueltank.ViewModels
{

    public class MainPageViewModel : ViewModel
    {
        private RelayCommand<NavigationViewItemInvokedEventArgs > _navigationItemInvoked;
        private string _hamburgerTitle = "Home";

        public MainPageViewModel()
        {
            AppTitle = "DuelTank";
        }

        public string AppTitle { get; }

        public string HamburgerTitle
        {
            get => _hamburgerTitle;
            set
            {
                _hamburgerTitle = value;

                //raise property changed event
                OnPropertyChanged();
            }
        }

        public RelayCommand<NavigationViewItemInvokedEventArgs> NavigationItemInvoked
        {
            get
            {
                return _navigationItemInvoked ?? (_navigationItemInvoked = new RelayCommand<NavigationViewItemInvokedEventArgs>(OnNavigationItemInvoked, param => true));
            }
            set => _navigationItemInvoked = value;
        }

        private void OnNavigationItemInvoked(NavigationViewItemInvokedEventArgs navigationViewItemInvokedEventArgs)
        {
            HamburgerTitle = (string)navigationViewItemInvokedEventArgs.InvokedItem;
        }
    }
}