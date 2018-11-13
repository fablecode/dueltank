using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Models.DeckDetails;
using dueltank.core.Models.Search.Decks;
using dueltank.core.Models.YgoPro;

namespace dueltank.core.Services
{
    public interface IDeckService
    {
        Task<Deck> Add(YgoProDeck ygoProDeck);
        Task<Deck> Add(DeckModel deckModel);
        Task<DeckDetail> GetDeckById(long id);
        Task<DeckSearchResult> Search(DeckSearchCriteria searchCriteria);
        Task<MostRecentDecksResult> MostRecentDecks(int pageSize);
    }
}