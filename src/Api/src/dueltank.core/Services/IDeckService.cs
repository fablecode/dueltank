using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Models.YgoPro;

namespace dueltank.core.Services
{
    public interface IDeckService
    {
        Task<Deck> Add(YgoProDeck ygoProDeck);
    }
}