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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace dueltank.ViewModels.Shell
{

    public class MainShellViewModel : ShellViewModel
    {
        private readonly INavigationService _navigationService;
        private string _hamburgerTitle = "Home";
        private bool _isPaneOpen;
        private object _selectedItem;

        private RelayCommand<NavigationViewSelectionChangedEventArgs> _navigationItemInvoked;
        private RelayCommand<RoutedEventArgs> _backButtonInvoked;

        private readonly NavigationItem _homeItem = new NavigationItem(0xE80F, "Home", typeof(HomeViewModel));
        private readonly NavigationItem _decksItem = new NavigationItem(0xECAA, "Decks", typeof(DecksViewModel));
        private readonly NavigationItem _banlistItem = new NavigationItem(0xE876, "Banlist", typeof(BanlistViewModel));
        private readonly NavigationItem _cardsItem = new NavigationItem(0xE81E, "Cards", typeof(CardsViewModel));
        private readonly NavigationItem _archetypesItem = new NavigationItem(0xE8EC, "Archetypes", typeof(ArchetypesViewModel));

        public MainShellViewModel()
            : this(ServiceLocator.Current.GetService<INavigationService>())
        {
            AppTitle = "DuelTank";
        }
        public MainShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
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

        public object SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
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



        #endregion

        public void NavigateTo(Type viewModel)
        {
            _navigationService.Navigate(viewModel);
        }
    }
}