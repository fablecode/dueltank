using dueltank.ViewModels.Infrastructure.Services;

namespace dueltank.Services.Infrastructure
{
    public class CommonServices : ICommonServices
    {
        public CommonServices(IContextService contextService, INavigationService navigationService, IMessageService messageService)
        {
            ContextService = contextService;
            NavigationService = navigationService;
            MessageService = messageService;
        }

        public IContextService ContextService { get; }

        public INavigationService NavigationService { get; }

        public IMessageService MessageService { get; }
    }
}