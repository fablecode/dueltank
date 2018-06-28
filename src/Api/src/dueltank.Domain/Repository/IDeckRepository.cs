using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Models.Decks;

namespace dueltank.Domain.Repository
{
    public interface IDeckRepository
    {
        Task<Deck> Add(Deck ygoProDeck);
        Task<DeckDetail> GetDeckById(long id);
    }
}