using System.Threading.Tasks;
using dueltank.core.Models.Db;

namespace dueltank.Domain.Repository
{
    public interface ICardRepository
    {
        Task<Card> GetCardByNumber(long cardNumber);
    }
}