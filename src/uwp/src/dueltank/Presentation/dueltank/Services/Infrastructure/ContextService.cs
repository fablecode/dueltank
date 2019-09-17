using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using dueltank.ViewModels.Infrastructure.Services;

namespace dueltank.Services.Infrastructure
{
    public class ContextService : IContextService
    {
        private static int _mainViewId = -1;

        private CoreDispatcher _dispatcher = null;

        public int MainViewId => _mainViewId;

        public int ContextId { get; private set; }

        public bool IsMainView { get; private set; }

        public void Initialize(object dispatcher, int contextId, bool isMainView)
        {
            _dispatcher = dispatcher as CoreDispatcher;
            ContextId = contextId;
            IsMainView = isMainView;
            if (IsMainView)
            {
                _mainViewId = ContextId;
            }
        }

        public async Task RunAsync(Action action)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}