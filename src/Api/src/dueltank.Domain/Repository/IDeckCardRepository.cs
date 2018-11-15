using System.Threading.Tasks;

namespace dueltank.Domain.Repository
{
    public interface IDeckCardRepository
    {
        Task DeleteDeckCardsByDeckId(long deckId);
    }
}