using System.Threading.Tasks;
using dueltank.core.Services;
using dueltank.Domain.Repository;

namespace dueltank.Domain.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<bool> IsUserDeckOwner(string userId, long deckId)
        {
            return _userRepository.IsUserDeckOwner(userId, deckId);
        }
    }
}