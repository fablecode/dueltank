using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;
using dueltank.core.Models.Decks;
using dueltank.Domain.Repository;
using dueltank.infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace dueltank.infrastructure.Repository
{
    public class DeckRepository : IDeckRepository
    {
        private readonly DueltankDbContext _dbContext;

        public DeckRepository(DueltankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Deck> Add(Deck ygoProDeck)
        {
            await _dbContext.Deck.AddAsync(ygoProDeck);

            await _dbContext.SaveChangesAsync();

            return ygoProDeck;
        }

        public async Task<DeckDetail> GetDeckById(long id)
        {
            var deckResult = await _dbContext.Deck.AsNoTracking().Include(d => d.User).SingleOrDefaultAsync(d => d.Id == id);

            var cardResult = _dbContext.CardDetail.FromSql("usp_GetDeckCardsByDeckId @DeckId", new SqlParameter("@DeckId", id)).ToList();

            if (deckResult != null)
            {
                var deckDetail = DeckDetail.From(deckResult);

                //var deckCards = await cardQuery.ToListAsync();

                var groupedDeckCards = cardResult.GroupBy(dt => dt.DeckType, dt => dt);

                foreach (var cardGroup in groupedDeckCards)
                {
                    switch (cardGroup.Key.ToLower())
                    {
                        case "main":
                            deckDetail.MainDeck = cardGroup.ToList();
                            break;
                        case "extra":
                            deckDetail.ExtraDeck = cardGroup.ToList();
                            break;
                        case "side":
                            deckDetail.SideDeck = cardGroup.ToList();
                            break;
                    }
                }

                return deckDetail;
            }

            return null;
        }
    }
}