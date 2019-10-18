using System.Threading.Tasks;

namespace dueltank.ViewModels.Infrastructure
{
    public interface IAccountService
    {
        void SignIn();
        Task SignOut();
    }
}