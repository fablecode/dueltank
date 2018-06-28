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
            var deckResult = await _dbContext.Deck.AsNoTracking().Include(d => d.User).SingleOrDefaultAsync();

            var cardQuery =
            (
                from d in _dbContext.Deck
                join dc in _dbContext.DeckCard on d.Id equals dc.DeckId
                join dt in _dbContext.DeckType on dc.DeckTypeId equals dt.Id
                join c in _dbContext.Card on dc.CardId equals c.Id
                join csc in _dbContext.CardSubCategory on c.Id equals csc.CardId
                join sc in _dbContext.SubCategory on csc.SubCategoryId equals sc.Id
                join cat in _dbContext.Category on sc.CategoryId equals cat.Id
                from ca in _dbContext.CardAttribute.DefaultIfEmpty()
                from a in _dbContext.Attribute.DefaultIfEmpty()
                from ct in _dbContext.CardType.DefaultIfEmpty()
                from t in _dbContext.Type.DefaultIfEmpty()
                where d.Id == id
                orderby dc.SortOrder
                select new CardDetail
                {
                    Id = c.Id,
                    DeckType = dt.Name,
                    Name = c.Name,
                    CardNumber = c.CardNumber,
                    Description = c.Description,
                    CardLevel = c.CardLevel,
                    CardRank = c.CardRank,
                    Atk = c.Atk,
                    Def = c.Def,
                    Created = c.Created,
                    Updated = c.Updated,
                    CategoryId = cat.Id,
                    Category = cat.Name,
                    SubCategories = string.Join
                    (
                        ",",
                        from sc2 in _dbContext.SubCategory
                        join csc2 in _dbContext.CardSubCategory on sc2.Id equals csc2.SubCategoryId
                        where csc2.CardId == c.Id
                        select sc2.Name
                    ),
                    Attribute = a.Name,
                    TypeId = t.Id,
                    Quantity = dc.Quantity,
                    SortOrder = dc.SortOrder
                }
            );

            if (deckResult != null)
            {
                var deckDetail = DeckDetail.From(deckResult);

                var deckCards = await cardQuery.ToListAsync();

                var groupedDeckCards = deckCards.GroupBy(dt => dt.DeckType, dt => dt);

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