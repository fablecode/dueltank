using dueltank.ViewModels.Archetypes;
using dueltank.ViewModels.Banlist;
using dueltank.ViewModels.Cards;
using dueltank.ViewModels.Decks;
using dueltank.ViewModels.Infrastructure.Common;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using dueltank.ViewModels.Home;

namespace dueltank.ViewModels.Shell
{

    public class MainShellViewModel : ShellViewModel
    {
        private RelayCommand<NavigationViewItemInvokedEventArgs> _navigationItemInvoked;
        private string _hamburgerTitle = "Home";
        private bool _isPaneOpen;

        private readonly NavigationItem _dashboardItem = new NavigationItem(0xE80F, "Home", typeof(HomeViewModel));
        private readonly NavigationItem _decksItem = new NavigationItem(0xECAA, "Decks", typeof(DecksViewModel));
        private readonly NavigationItem _banlistItem = new NavigationItem(0xE876, "Banlist", typeof(BanlistViewModel));
        private readonly NavigationItem _cardsItem = new NavigationItem(0xE81E, "Cards", typeof(CardsViewModel));
        private readonly NavigationItem _archetypesItem = new NavigationItem(0xE8EC, "Archetypes", typeof(ArchetypesViewModel));

        public MainShellViewModel()
        {
            AppTitle = "DuelTank";
        }

        public string AppTitle { get; }

        public IEnumerable<NavigationItem> NavigationViewItems
        {
            get
            {
                yield return _dashboardItem;
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

        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
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