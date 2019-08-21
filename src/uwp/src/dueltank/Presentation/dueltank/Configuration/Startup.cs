using System.Threading.Tasks;
using dueltank.Services.Infrastructure;
using dueltank.ViewModels.Shell;
using dueltank.Views.Shell;
using Microsoft.Extensions.DependencyInjection;

namespace dueltank.Configuration
{
    public static class Startup
    {
        private static readonly ServiceCollection _serviceCollection = new ServiceCollection();

        static public async Task ConfigureAsync()
        {
            ServiceLocator.Configure(_serviceCollection);

            ConfigureNavigation();
        }

        private static void ConfigureNavigation()
        {
            NavigationService.Register<MainShellViewModel, MainShellView>();
        }
    }
}