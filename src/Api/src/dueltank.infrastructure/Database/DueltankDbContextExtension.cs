using dueltank.core.Models.Cards;
using dueltank.core.Models.Search.Banlist;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Database
{
    public partial class DueltankDbContext
    {
        public virtual DbSet<DeckCardDetail> CardDetail { get; set; }
        public DbSet<CardSearch> CardSearch { get; set; }
        public DbSet<BanlistCardSearch> BanlistCardSearch { get; set; }
    }
}