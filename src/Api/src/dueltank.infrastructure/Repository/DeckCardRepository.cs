using System.Linq;
using System.Threading.Tasks;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;

namespace dueltank.infrastructure.Repository
{
    public class DeckCardRepository : IDeckCardRepository
    {
        private readonly DueltankDbContext _dbContext;

        public DeckCardRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task DeleteDeckCardsByDeckId(long deckId)
        {
            _dbContext.DeckCard.RemoveRange(from dc in _dbContext.DeckCard where dc.DeckId == deckId select dc);

            return _dbContext.SaveChangesAsync();
        }
    }
}