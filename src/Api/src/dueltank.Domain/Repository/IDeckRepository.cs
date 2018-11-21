using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Models.DeckDetails;
using dueltank.core.Models.Search.Decks;

namespace dueltank.Domain.Repository
{
    public interface IDeckRepository
    {
        Task<Deck> Add(Deck deck);
        Task<DeckDetail> GetDeckById(long id);
        Task<DeckSearchResult> Search(DeckSearchCriteria searchCriteria);
        Task<DeckSearchResult> Search(string userId, DeckSearchByUserIdCriteria searchCriteria);
        Task<MostRecentDecksResult> MostRecentDecks(int pageSize);
        Task<Deck> Update(Deck deck);
    }
}