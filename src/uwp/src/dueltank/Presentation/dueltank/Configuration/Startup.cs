﻿using System.Threading.Tasks;
using dueltank.Services.Infrastructure;
using dueltank.ViewModels.Accounts;
using dueltank.ViewModels.Decks;
using dueltank.ViewModels.Home;
using dueltank.ViewModels.Shell;
using dueltank.Views.AccountSettings;
using dueltank.Views.Decks;
using dueltank.Views.Home;
using dueltank.Views.Shell;
using dueltank.Views.Username;
using Microsoft.Extensions.DependencyInjection;

namespace dueltank.Configuration
{
    public static class Startup
    {
        private static readonly ServiceCollection _serviceCollection = new ServiceCollection();

        public static Task ConfigureAsync()
        {
            ServiceLocator.Configure(_serviceCollection);

            ConfigureNavigation();

            return Task.CompletedTask;
        }

        private static void ConfigureNavigation()
        {
            NavigationService.Register<MainShellViewModel, MainShellView>();
            NavigationService.Register<HomeViewModel, HomeView>();
            NavigationService.Register<DecksViewModel, DecksView>();
            NavigationService.Register<UsernameViewModel, UsernameView>();
            NavigationService.Register<AccountSettingsViewModel, AccountSettingsView>();
        }
    }
}