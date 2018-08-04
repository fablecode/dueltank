using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class LimitRepository : ILimitRepository
    {
        private readonly DueltankDbContext _dbContext;

        public LimitRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Limit>> AllLimits()
        {
            return await _dbContext.Limit.ToListAsync();
        }
    }
}