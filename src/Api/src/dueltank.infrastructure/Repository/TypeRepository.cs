using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class TypeRepository : ITypeRepository
    {
        private readonly DueltankDbContext _dbContext;

        public TypeRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IList<Type>> AllTypes()
        {
            return await _dbContext.Type.ToListAsync();
        }
    }
}