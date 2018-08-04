using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly DueltankDbContext _dbContext;

        public SubCategoryRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<SubCategory>> AllSubCategories()
        {
            return await _dbContext.SubCategory.ToListAsync();
        }
    }
}