using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class TipRepository : ITipRepository
    {
        private readonly DueltankDbContext _dbContext;

        public TipRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<TipSection>> GetByCardId(long cardId)
        {
            return await _dbContext
                .TipSection
                .AsNoTracking()
                .Include(ts => ts.Tip)
                .Where(ts => ts.CardId == cardId)
                .ToListAsync();
        }
    }
}