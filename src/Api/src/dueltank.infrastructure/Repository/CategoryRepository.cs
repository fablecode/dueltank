using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DueltankDbContext _dbContext;

        public CategoryRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Category>> AllCategories()
        {
            return await _dbContext.Category.ToListAsync();
        }
    }
}