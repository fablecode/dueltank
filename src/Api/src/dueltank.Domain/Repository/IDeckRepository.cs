using System.Threading.Tasks;
using dueltank.core.Models.Db;

namespace dueltank.Domain.Repository
{
    public interface IDeckRepository
    {
        Task<Deck> Add(Deck ygoProDeck);
    }
}