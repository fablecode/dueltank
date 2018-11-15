using System.Threading.Tasks;

namespace dueltank.Domain.Repository
{
    public interface IUserRepository
    {
        Task<bool> IsUserDeckOwner(string userId, long deckId);
    }
}