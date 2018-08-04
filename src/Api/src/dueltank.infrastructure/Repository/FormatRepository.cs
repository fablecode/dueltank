using dueltank.core.Models.Db;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dueltank.infrastructure.Repository
{
    public class FormatRepository : IFormatRepository
    {
        private readonly DueltankDbContext _context;

        public FormatRepository(DueltankDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Format>> AllFormats()
        {
            return await _context
                            .Format
                            .Include(b => b.Banlist)
                                .ThenInclude(b => b.BanlistCard)
                                    .ThenInclude(b => b.Limit)
                            .ToListAsync();
        }
    }
}