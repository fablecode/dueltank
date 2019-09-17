namespace dueltank.ViewModels.Infrastructure.Services
{
    public interface ICommonServices
    {
        IContextService ContextService { get; }
        INavigationService NavigationService { get; }
        IMessageService MessageService { get; }
    }
}