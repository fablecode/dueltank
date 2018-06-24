using System.Threading.Tasks;
using dueltank.core.Models.Db;

namespace dueltank.core.Services
{
    public interface ICardService
    {
        Task<Card> GetCardByNumber(string cardNumber);
    }
}