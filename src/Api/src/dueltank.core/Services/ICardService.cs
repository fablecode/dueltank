using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Models.Search.Card;

namespace dueltank.core.Services
{
    public interface ICardService
    {
        Task<Card> GetCardByNumber(long cardNumber);
        Task<CardSearchResult> Search(CardSearchCriteria searchCriteria);
    }
}