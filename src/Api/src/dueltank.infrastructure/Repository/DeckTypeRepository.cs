using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class DeckTypeRepository : IDeckTypeRepository
    {
        private readonly DueltankDbContext _dbContext;

        public DeckTypeRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DeckType>> AllDeckTypes()
        {
            return await _dbContext.DeckType.Select(c => c).ToListAsync();
        }
    }
}