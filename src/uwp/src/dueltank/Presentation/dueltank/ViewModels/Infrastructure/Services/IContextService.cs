using System;
using System.Threading.Tasks;

namespace dueltank.ViewModels.Infrastructure.Services
{
    public interface IContextService
    {
        int MainViewId { get; }

        int ContextId { get; }

        bool IsMainView { get; }

        void Initialize(object dispatcher, int contextId, bool isMainView);

        Task RunAsync(Action action);
    }
}