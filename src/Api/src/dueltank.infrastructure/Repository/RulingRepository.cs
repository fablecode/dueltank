using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class RulingRepository : IRulingRepository
    {
        private readonly DueltankDbContext _dbContext;

        public RulingRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<RulingSection>> GetByCardId(long cardId)
        {
            return await _dbContext
                .RulingSection
                .AsNoTracking()
                .Include(r => r.Ruling)
                .Where(r => r.CardId == cardId)
                .ToListAsync();
        }
    }
}