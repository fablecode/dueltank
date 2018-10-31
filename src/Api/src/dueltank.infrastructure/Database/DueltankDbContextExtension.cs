using Microsoft.EntityFrameworkCore;
using ArchetypeSearch = dueltank.core.Models.Archetypes.ArchetypeSearch;
using BanlistCardSearch = dueltank.core.Models.Search.Banlists.BanlistCardSearch;
using MostRecentBanlist = dueltank.core.Models.Search.Banlists.MostRecentBanlist;
using CardSearch = dueltank.core.Models.Cards.CardSearch;
using DeckCardDetail = dueltank.core.Models.Cards.DeckCardDetail;
using DeckDetail = dueltank.core.Models.DeckDetails.DeckDetail;


namespace dueltank.infrastructure.Database
{
    public partial class DueltankDbContext
    {
        public virtual DbSet<DeckCardDetail> CardDetail { get; set; }
        public virtual DbSet<DeckDetail> DeckDetail { get; set; }
        public virtual DbSet<CardSearch> CardSearch { get; set; }
        public virtual DbSet<BanlistCardSearch> BanlistCardSearch { get; set; }
        public virtual DbSet<MostRecentBanlist> MostRecentBanlist { get; set; }
        public virtual DbSet<ArchetypeSearch> ArchetypeSearch { get; set; }
    }
}