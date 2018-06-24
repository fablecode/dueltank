using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;

namespace dueltank.infrastructure.Repository
{
    public class DeckRepository : IDeckRepository
    {
        private readonly DueltankDbContext _dbContext;

        public DeckRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Deck> Add(Deck ygoProDeck)
        {
            await _dbContext.Deck.AddAsync(ygoProDeck);

            await _dbContext.SaveChangesAsync();

            return ygoProDeck;
        }
    }
}