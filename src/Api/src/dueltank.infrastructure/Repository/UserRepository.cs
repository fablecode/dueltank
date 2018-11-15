using System.Linq;
using System.Threading.Tasks;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DueltankDbContext _dbContext;

        public UserRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsUserDeckOwner(string userId, long deckId)
        {
            var result = await
            (
                from d in _dbContext.Deck
                where d.Id == deckId && d.UserId == userId
                select d
            )
            .Take(1)
            .AnyAsync();

            return result;
        }
    }
}