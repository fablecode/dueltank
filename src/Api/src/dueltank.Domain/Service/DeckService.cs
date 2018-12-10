using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using dueltank.core.Constants;
using dueltank.core.Models.Db;
using dueltank.core.Models.DeckDetails;
using dueltank.core.Models.Search.Decks;
using dueltank.core.Models.YgoPro;
using dueltank.core.Services;
using dueltank.Domain.Repository;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;

namespace dueltank.Domain.Service
{
    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IDeckTypeRepository _deckTypeRepository;
        private readonly IDeckCardRepository _deckCardRepository;

        public DeckService
        (
            IDeckRepository deckRepository, 
            ICardRepository cardRepository, 
            IDeckTypeRepository deckTypeRepository,
            IDeckCardRepository  deckCardRepository)
        {
            _deckRepository = deckRepository;
            _cardRepository = cardRepository;
            _deckTypeRepository = deckTypeRepository;
            _deckCardRepository = deckCardRepository;
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

            newDeck = await AddCardsToDeckByCardNumber(mainDeckUniqueCards, newDeck, deckTypes[DeckTypeConstants.Main], mainDeckCardCopies);
            newDeck = await AddCardsToDeckByCardNumber(extraDeckUniqueCards, newDeck, deckTypes[DeckTypeConstants.Extra], extraDeckCardCopies);
            newDeck = await AddCardsToDeckByCardNumber(sideDeckUniqueCards, newDeck, deckTypes[DeckTypeConstants.Side], sideDeckCardCopies);

            await _deckRepository.Add(newDeck);

            return newDeck;
        }

        public async Task<Deck> Add(DeckModel deckModel)
        {
            var newDeck = new Deck
            {
                UserId = deckModel.UserId,
                Name = deckModel.Name,
                Description = deckModel.Description,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            var deckTypes = (await _deckTypeRepository.AllDeckTypes()).ToDictionary(dt => dt.Name, dt => dt);

            var mainDeckUniqueCards = deckModel.MainDeck.Select(c => c.Id).Distinct().ToList();
            var extraDeckUniqueCards = deckModel.ExtraDeck.Select(c => c.Id).Distinct().ToList();
            var sideDeckUniqueCards = deckModel.SideDeck.Select(c => c.Id).Distinct().ToList();

            var mainDeckCardCopies = deckModel.MainDeck.GroupBy(c => c.Id).ToDictionary(g => g.Key, g => g.Count());
            var extraDeckCardCopies = deckModel.ExtraDeck.GroupBy(c => c.Id).ToDictionary(g => g.Key, g => g.Count());
            var sideDeckCardCopies = deckModel.SideDeck.GroupBy(c => c.Id).ToDictionary(g => g.Key, g => g.Count());

            newDeck = await AddCardsToDeckByCardNumber(mainDeckUniqueCards, newDeck, deckTypes[DeckTypeConstants.Main], mainDeckCardCopies);
            newDeck = await AddCardsToDeckByCardNumber(extraDeckUniqueCards, newDeck, deckTypes[DeckTypeConstants.Extra], extraDeckCardCopies);
            newDeck = await AddCardsToDeckByCardNumber(sideDeckUniqueCards, newDeck, deckTypes[DeckTypeConstants.Side], sideDeckCardCopies);

            await _deckRepository.Add(newDeck);

            return newDeck;
        }

        public async Task<Deck> Update(DeckModel deckModel)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _deckCardRepository.DeleteDeckCardsByDeckId(deckModel.Id.GetValueOrDefault());

                var deckSearch = await _deckRepository.GetDeckById(deckModel.Id.GetValueOrDefault());

                var existingDeck = new Deck
                {
                    Id = deckSearch.Id,
                    UserId = deckSearch.UserId,
                    Name = deckModel.Name,
                    Description = deckModel.Description,
                    VideoUrl = deckModel.VideoUrl,
                    Created = deckSearch.Created,
                    Updated = DateTime.UtcNow
                };

                var deckTypes = (await _deckTypeRepository.AllDeckTypes()).ToDictionary(dt => dt.Name, dt => dt);

                var mainDeckUniqueCards = deckModel.MainDeck.Select(c => c.Id).Distinct().ToList();
                var extraDeckUniqueCards = deckModel.ExtraDeck.Select(c => c.Id).Distinct().ToList();
                var sideDeckUniqueCards = deckModel.SideDeck.Select(c => c.Id).Distinct().ToList();

                var mainDeckCardCopies = deckModel.MainDeck.GroupBy(c => c.Id).ToDictionary(g => g.Key, g => g.Count());
                var extraDeckCardCopies = deckModel.ExtraDeck.GroupBy(c => c.Id).ToDictionary(g => g.Key, g => g.Count());
                var sideDeckCardCopies = deckModel.SideDeck.GroupBy(c => c.Id).ToDictionary(g => g.Key, g => g.Count());

                existingDeck = await AddCardsToDeckByCardId(mainDeckUniqueCards, existingDeck, deckTypes[DeckTypeConstants.Main], mainDeckCardCopies);
                existingDeck = await AddCardsToDeckByCardId(extraDeckUniqueCards, existingDeck, deckTypes[DeckTypeConstants.Extra], extraDeckCardCopies);
                existingDeck = await AddCardsToDeckByCardId(sideDeckUniqueCards, existingDeck, deckTypes[DeckTypeConstants.Side], sideDeckCardCopies);


                var updateResult = await _deckRepository.Update(existingDeck);

                scope.Complete();

                return updateResult;
            }
        }

        private async Task<Deck> AddCardsToDeckByCardNumber(List<long> uniqueCards, Deck newDeck, DeckType deckType, IReadOnlyDictionary<long, int> cardCopies)
        {
            return await AddCardsToDeck(uniqueCards, newDeck, deckType, cardCopies, _cardRepository.GetCardByNumber, (cardIds, card) => cardIds.FindIndex(c => c == card.CardNumber) + 1);
        }
        private async Task<Deck> AddCardsToDeckByCardId(List<long> uniqueCards, Deck newDeck, DeckType deckType, IReadOnlyDictionary<long, int> cardCopies)
        {
            return await AddCardsToDeck(uniqueCards, newDeck, deckType, cardCopies, _cardRepository.GetCardById, (cardIds, card) => cardIds.FindIndex(c => c == card.Id) + 1);
        }

        private static async Task<Deck> AddCardsToDeck(List<long> uniqueCards, Deck deck, DeckType deckType, IReadOnlyDictionary<long, int> cardCopies, Func<long, Task<Card>> getCardFunc, Func<List<long>, Card, int> cardSortOrderFunc)
        {
            foreach (var id in uniqueCards)
            {
                var cardResult = await getCardFunc(id);

                if (cardResult == null)
                    continue;

                deck.DeckCard.Add(new DeckCard
                {
                    DeckType = deckType,
                    Deck = deck,
                    CardId = cardResult.Id,
                    Quantity = cardCopies[id],
                    SortOrder = cardSortOrderFunc(uniqueCards, cardResult)
                });
            }

            return deck;
        }

        public Task<DeckDetail> GetDeckById(long id)
        {
            return _deckRepository.GetDeckById(id);
        }

        public Task<DeckSearchResult> Search(DeckSearchCriteria searchCriteria)
        {
            return _deckRepository.Search(searchCriteria);
        }

        public Task<DeckSearchResult> Search(DeckSearchByUserIdCriteria searchCriteria)
        {
            return _deckRepository.Search(searchCriteria);
        }

        public Task<DeckSearchResult> Search(DeckSearchByUsernameCriteria searchCriteria)
        {
            return _deckRepository.Search(searchCriteria);
        }

        public Task<MostRecentDecksResult> MostRecentDecks(int pageSize)
        {
            return _deckRepository.MostRecentDecks(pageSize);
        }

        public long SaveDeckThumbnail(DeckThumbnail deckThumbnailModel)
        {
            var quality = 90;
            ISupportedImageFormat format = new PngFormat();
            var size = new Size(170, 0);

            using (var inStream = new MemoryStream(deckThumbnailModel.Thumbnail))
            {
                using (var imageFactory = new ImageFactory())
                {
                    // Load, resize, set the format and quality and save an image.
                    imageFactory
                        .Load(inStream)
                        .Resize(size)
                        .Format(format)
                        .Quality(quality)
                        .Save(deckThumbnailModel.ImageFilePath);
                }
            }

            return deckThumbnailModel.DeckId;
        }

        public Task<long> DeleteDeckByIdAndUserId(string userId, long deckId)
        {
            return _deckRepository.DeleteDeckByIdAndUserId(userId, deckId);
        }
    }
}