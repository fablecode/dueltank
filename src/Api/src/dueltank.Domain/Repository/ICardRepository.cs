using System.Threading.Tasks;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;
using dueltank.core.Models.Search.Card;

namespace dueltank.Domain.Repository
{
    public interface ICardRepository
    {
        Task<Card> GetCardByNumber(long cardNumber);
        Task<CardSearchResult> Search(CardSearchCriteria searchCriteria);
        Task<CardSearch> GetCardByName(string name);
    }
}