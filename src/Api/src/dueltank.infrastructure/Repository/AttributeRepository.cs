using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class AttributeRepository : IAttributeRepository
    {
        private readonly DueltankDbContext _dbContext;

        public AttributeRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Attribute>> AllAttributes()
        {
            return await _dbContext.Attribute.ToListAsync();
        }
    }
}