using System;
using System.Linq;
using System.Threading.Tasks;
using dueltank.core.Models.Db;
using dueltank.core.Models.YgoPro;
using dueltank.core.Services;
using dueltank.Domain.Repository;

namespace dueltank.Domain.Service
{
    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;
        private readonly ICardRepository _cardRepository;

        public DeckService(IDeckRepository deckRepository, ICardRepository cardRepository)
        {
            _deckRepository = deckRepository;
            _cardRepository = cardRepository;
        }

        public async Task<Deck> Add(YgoProDeck ygoProDeck)
        {
            var newDeck = new Deck
            {
                UserId = ygoProDeck.UserId.ToString(),
                Name = ygoProDeck.Name,
                Description = ygoProDeck.Description,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            var mainDeckUniqueCards = ygoProDeck.Main.Select(c => c).Distinct().ToList();
            var extraDeckUniqueCards = ygoProDeck.Extra.Select(c => c).Distinct().ToList();
            var sideDeckUniqueCards = ygoProDeck.Side.Select(c => c).Distinct().ToList();

            var mainDeckCardCopies = ygoProDeck.Main.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
            var extraDeckCardCopies = ygoProDeck.Extra.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
            var sideDeckCardCopies = ygoProDeck.Side.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());

            foreach (var cardNumber in mainDeckUniqueCards)
            {
                var cardResult = await _cardRepository.GetCardByNumber(cardNumber);

                if (cardResult == null)
                    continue;

                newDeck.DeckCard.Add(new DeckCard
                {
                    CardId = cardResult.Id,
                    Deck = newDeck,

                });
            }

            return newDeck;
        }

        private string AddLeadingZerosToCardNumber(string cardNumber)
        {
            return long.Parse(cardNumber).ToString("D8");
        }
    }
}