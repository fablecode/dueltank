using dueltank.ViewModels.Infrastructure.Services;
using dueltank.ViewModels.Shell;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using dueltank.Views.Shell;

namespace dueltank.Services.Infrastructure
{
    public class NavigationService : INavigationService
    {
        private static readonly ConcurrentDictionary<Type, Type> ViewModelMap = new ConcurrentDictionary<Type, Type>();

        static NavigationService()
        {
            MainViewId = ApplicationView.GetForCurrentView().Id;
        }

        public static int MainViewId { get; }

        public static void Register<TViewModel, TView>() where TView : Page
        {
            if (!ViewModelMap.TryAdd(typeof(TViewModel), typeof(TView)))
            {
                throw new InvalidOperationException($"ViewModel already registered '{typeof(TViewModel).FullName}'");
            }
        }

        public static Type GetView<TViewModel>()
        {
            return GetView(typeof(TViewModel));
        }
        public static Type GetView(Type viewModel)
        {
            if (ViewModelMap.TryGetValue(viewModel, out Type view))
            {
                return view;
            }
            throw new InvalidOperationException($"View not registered for ViewModel '{viewModel.FullName}'");
        }

        public static Type GetViewModel(Type view)
        {
            var type = ViewModelMap.Where(r => r.Value == view).Select(r => r.Key).FirstOrDefault();
            if (type == null)
            {
                throw new InvalidOperationException($"View not registered for ViewModel '{view.FullName}'");
            }
            return type;
        }

        public bool IsMainView => CoreApplication.GetCurrentView().IsMain;

        public Frame Frame { get; private set; }

        public bool CanGoBack => Frame.CanGoBack;

        public void GoBack() => Frame.GoBack();

        public void Initialize(object frame)
        {
            Frame = frame as Frame;
        }

        public bool Navigate<TViewModel>(object parameter = null)
        {
            return Navigate(typeof(TViewModel), parameter);
        }
        public bool Navigate(Type viewModelType, object parameter = null)
        {
            if (Frame == null)
            {
                throw new InvalidOperationException("Navigation frame not initialized.");
            }
            return Frame.Navigate(GetView(viewModelType), parameter);
        }

        public async Task<int> CreateNewViewAsync<TViewModel>(object parameter = null)
        {
            return await CreateNewViewAsync(typeof(TViewModel), parameter);
        }
        public async Task<int> CreateNewViewAsync(Type viewModelType, object parameter = null)
        {
            int viewId = 0;

            var newView = CoreApplication.CreateNewView();
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                viewId = ApplicationView.GetForCurrentView().Id;

                var frame = new Frame();
                var args = new ShellArgs
                {
                    ViewModel = viewModelType,
                    Parameter = parameter
                };
                frame.Navigate(typeof(MainShellView), args);

                Window.Current.Content = frame;
                Window.Current.Activate();
            });

            if (await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId))
            {
                return viewId;
            }

            return 0;
        }

        public async Task CloseViewAsync()
        {
            int currentId = ApplicationView.GetForCurrentView().Id;
            await ApplicationViewSwitcher.SwitchAsync(MainViewId, currentId, ApplicationViewSwitchingOptions.ConsolidateViews);
        }
    }
}