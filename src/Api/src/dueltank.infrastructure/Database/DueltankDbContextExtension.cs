using dueltank.core.Models.Cards;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Database
{
    public partial class DueltankDbContext
    {
        public virtual DbSet<DeckCardDetail> CardDetail { get; set; }
        public DbSet<CardSearch> CardSearch { get; set; }
    }
}