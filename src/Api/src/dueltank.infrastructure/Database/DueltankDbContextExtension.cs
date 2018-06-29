using dueltank.core.Models.Cards;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Database
{
    public partial class DueltankDbContext
    {
        public virtual DbSet<CardDetail> CardDetail { get; set; }
    }
}