using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Models.Search.Card;
using dueltank.core.Services;
using dueltank.Domain.Repository;

namespace dueltank.Domain.Service
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;

        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public Task<Card> GetCardByNumber(long cardNumber)
        {
            return _cardRepository.GetCardByNumber(cardNumber);
        }

        public Task<CardSearchResult> Search(CardSearchCriteria searchCriteria)
        {
            return _cardRepository.Search(searchCriteria);
        }
    }
}