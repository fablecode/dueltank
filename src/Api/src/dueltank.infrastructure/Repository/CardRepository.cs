using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly DueltankDbContext _dbContext;

        public CardRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Card> GetCardByNumber(string cardNumber)
        {
            return await _dbContext.Card
                            .AsNoTracking()
                            .SingleOrDefaultAsync(c => c.CardNumber == cardNumber);
        }
    }
}