using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Models.DeckDetails;
using dueltank.core.Models.Search.Decks;

namespace dueltank.Domain.Repository
{
    public interface IDeckRepository
    {
        Task<Deck> Add(Deck ygoProDeck);
        Task<DeckDetail> GetDeckById(long id);
        Task<DeckSearchResult> Search(DeckSearchCriteria searchCriteria);
    }
}