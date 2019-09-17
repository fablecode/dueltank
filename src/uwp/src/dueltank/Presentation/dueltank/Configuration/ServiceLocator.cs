using System;
using System.Collections.Concurrent;
using Windows.UI.ViewManagement;
using dueltank.Services.Infrastructure;
using dueltank.ViewModels.Archetypes;
using dueltank.ViewModels.Banlist;
using dueltank.ViewModels.Cards;
using dueltank.ViewModels.Decks;
using dueltank.ViewModels.Home;
using dueltank.ViewModels.Infrastructure.Services;
using dueltank.ViewModels.Shell;
using Microsoft.Extensions.DependencyInjection;

namespace dueltank.Configuration
{
    public class ServiceLocator
    {
        private static readonly ConcurrentDictionary<int, ServiceLocator> ServiceLocators = new ConcurrentDictionary<int, ServiceLocator>();

        private static ServiceProvider _rootServiceProvider;


        public static ServiceLocator Current
        {
            get
            {
                var currentViewId = ApplicationView.GetForCurrentView().Id;
                return ServiceLocators.GetOrAdd(currentViewId, key => new ServiceLocator());
            }
        }

        public static void DisposeCurrent()
        {
            var currentViewId = ApplicationView.GetForCurrentView().Id;
            if (ServiceLocators.TryRemove(currentViewId, out var current))
            {
                current.Dispose();
            }
        }

        public static void Configure(IServiceCollection serviceCollection)
        {
            // Services
            serviceCollection.AddScoped<INavigationService, NavigationService>();
            serviceCollection.AddScoped<IMessageService, MessageService>();
            serviceCollection.AddScoped<IContextService, ContextService>();
            serviceCollection.AddScoped<ICommonServices, CommonServices>();

            // ViewModels
            serviceCollection.AddTransient<ArchetypesViewModel>();
            serviceCollection.AddTransient<BanlistViewModel>();
            serviceCollection.AddTransient<CardsViewModel>();
            serviceCollection.AddTransient<DecksViewModel>();
            serviceCollection.AddTransient<HomeViewModel>();
            serviceCollection.AddTransient<ShellViewModel>();
            serviceCollection.AddTransient<MainShellViewModel>();

            _rootServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private readonly IServiceScope _serviceScope;

        private ServiceLocator()
        {
            _serviceScope = _rootServiceProvider.CreateScope();
        }

        public T GetService<T>()
        {
            return GetService<T>(true);
        }

        public T GetService<T>(bool isRequired)
        {
            if (isRequired)
            {
                return _serviceScope.ServiceProvider.GetRequiredService<T>();
            }
            return _serviceScope.ServiceProvider.GetService<T>();
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serviceScope?.Dispose();
            }
        }
        #endregion
    }
}