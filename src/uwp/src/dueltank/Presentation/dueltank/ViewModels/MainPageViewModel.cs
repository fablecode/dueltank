using dueltank.Helpers;
using dueltank.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace dueltank.ViewModels
{

    public class MainPageViewModel : ViewModel
    {
        private RelayCommand<NavigationViewItemInvokedEventArgs> _navigationItemInvoked;
        private string _hamburgerTitle = "Home";
        private const string SegoeMdl2Assets = "Segoe MDL2 Assets";

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

        public ObservableCollection<NavigationItemDataModel> NavigationViewItems =>
            new ObservableCollection<NavigationItemDataModel>
            {
                new NavigationItemDataModel { Content = "Home", FontFamily = SegoeMdl2Assets, Glyph = "\xE80F"},
                new NavigationItemDataModel { Content = "Banlist", FontFamily = SegoeMdl2Assets, Glyph = "\xE876"},
                new NavigationItemDataModel { Content = "Cards", FontFamily = SegoeMdl2Assets, Glyph = "\xE81E"},
                new NavigationItemDataModel { Content = "Decks", FontFamily = SegoeMdl2Assets, Glyph = "\xECAA"},
                new NavigationItemDataModel { Content = "Archetypes", FontFamily = SegoeMdl2Assets, Glyph = "\xE8EC"}
            };

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