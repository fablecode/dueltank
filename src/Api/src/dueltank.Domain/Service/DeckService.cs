using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dueltank.core.Constants;
using dueltank.core.Models.Db;
using dueltank.core.Models.Decks;
using dueltank.core.Models.Search.Decks;
using dueltank.core.Models.YgoPro;
using dueltank.core.Services;
using dueltank.Domain.Repository;

namespace dueltank.Domain.Service
{
    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IDeckTypeRepository _deckTypeRepository;

        public DeckService(IDeckRepository deckRepository, ICardRepository cardRepository, IDeckTypeRepository deckTypeRepository)
        {
            _deckRepository = deckRepository;
            _cardRepository = cardRepository;
            _deckTypeRepository = deckTypeRepository;
        }

        public async Task<Deck> Add(YgoProDeck ygoProDeck)
        {
            var newDeck = new Deck
            {
                UserId = ygoProDeck.UserId,
                Name = ygoProDeck.Name,
                Description = ygoProDeck.Description,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            var deckTypes = (await _deckTypeRepository.AllDeckTypes()).ToDictionary(dt => dt.Name, dt => dt);

            var mainDeckUniqueCards = ygoProDeck.Main.Select(c => c).Distinct().ToList();
            var extraDeckUniqueCards = ygoProDeck.Extra.Select(c => c).Distinct().ToList();
            var sideDeckUniqueCards = ygoProDeck.Side.Select(c => c).Distinct().ToList();

            var mainDeckCardCopies = ygoProDeck.Main.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
            var extraDeckCardCopies = ygoProDeck.Extra.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
            var sideDeckCardCopies = ygoProDeck.Side.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());

            newDeck = await AddCardsToDeck(mainDeckUniqueCards, newDeck, deckTypes[DeckTypeConstants.Main], mainDeckCardCopies);
            newDeck = await AddCardsToDeck(extraDeckUniqueCards, newDeck, deckTypes[DeckTypeConstants.Extra], extraDeckCardCopies);
            newDeck = await AddCardsToDeck(sideDeckUniqueCards, newDeck, deckTypes[DeckTypeConstants.Side], sideDeckCardCopies);

            await _deckRepository.Add(newDeck);

            return newDeck;
        }

        private async Task<Deck> AddCardsToDeck(List<long> uniqueCards, Deck newDeck, DeckType deckType, IReadOnlyDictionary<long, int> cardCopies)
        {
            foreach (var cardNumber in uniqueCards)
            {
                var cardResult = await _cardRepository.GetCardByNumber(cardNumber);

                if (cardResult == null)
                    continue;

                newDeck.DeckCard.Add(new DeckCard
                {
                    DeckType = deckType,
                    Deck = newDeck,
                    CardId = cardResult.Id,
                    Quantity = cardCopies[cardNumber],
                    SortOrder = uniqueCards.FindIndex(c => c == cardResult.CardNumber) + 1
                });
            }

            return newDeck;
        }

        public Task<DeckDetail> GetDeckById(long id)
        {
            return _deckRepository.GetDeckById(id);
        }

        public Task<DeckSearchResult> Search(DeckSearchCriteria searchCriteria)
        {
            return _deckRepository.Search(searchCriteria);
        }
    }
}