using System.Threading.Tasks;

namespace dueltank.core.Services
{
    public interface IUserService
    {
        Task<bool> IsUserDeckOwner(string userId, long deckId);
    }
}