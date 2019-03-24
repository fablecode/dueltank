using System;
using System.Linq;
using AutoMapper;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Constants;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;
using dueltank.core.Models.Search.Banlists;

namespace dueltank.application.Helpers
{
    public static class TrapCardHelper
    {
        public static bool IsTrapCard(DeckCardDetail deckCard)
        {
            return string.Equals(deckCard.Category, CardConstants.TrapType, StringComparison.OrdinalIgnoreCase);
        }
        public static bool IsTrapCard(CardSearch card)
        {
            return string.Equals(card.Category, CardConstants.TrapType, StringComparison.OrdinalIgnoreCase);
        }
        public static bool IsTrapCard(BanlistCardSearch card)
        {
            return string.Equals(card.Category, CardConstants.TrapType, StringComparison.OrdinalIgnoreCase);
        }

        public static Card MapToTrapCard(DeckCardDetail model)
        {
            var card = new Card
            {
                Id = model.Id,
                CardNumber = model.CardNumber,
                Name = model.Name,
                Description = model.Description,
                Created = model.Created,
                Updated = model.Updated
            };

            var subCategoryList = model.SubCategories.Split(',');

            foreach (var subCategory in subCategoryList)
            {
                card.CardSubCategory.Add(new CardSubCategory
                {
                    SubCategory = new SubCategory
                    {
                        Name = subCategory,
                        Category = new Category { Id = model.CategoryId, Name = model.Category }
                    }
                });
            }

            return card;
        }
        public static Card MapToTrapCard(CardSearch model)
        {
            var card = new Card
            {
                Id = model.Id,
                CardNumber = model.CardNumber,
                Name = model.Name,
                Description = model.Description,
            };

            var subCategoryList = model.SubCategories.Split(',');

            foreach (var subCategory in subCategoryList)
            {
                card.CardSubCategory.Add(new CardSubCategory
                {
                    SubCategory = new SubCategory
                    {
                        Name = subCategory,
                        Category = new Category { Id = model.CategoryId, Name = model.Category }
                    }
                });
            }

            return card;
        }
        public static Card MapToTrapCard(BanlistCardSearch model)
        {
            var card = new Card
            {
                Id = model.Id,
                CardNumber = model.CardNumber,
                Name = model.Name,
                Description = model.Description,
            };

            var subCategoryList = model.SubCategories.Split(',');

            foreach (var subCategory in subCategoryList)
            {
                card.CardSubCategory.Add(new CardSubCategory
                {
                    SubCategory = new SubCategory
                    {
                        Name = subCategory,
                        Category = new Category { Id = model.CategoryId, Name = model.Category }
                    }
                });
            }

            return card;
        }

        public static CardDetailOutputModel MapToCardOutputModel(IMapper mapper, DeckCardDetail deckCardSearch)
        {
            var card = MapToTrapCard(deckCardSearch);
            var cardOutputModel = mapper.Map<CardDetailOutputModel>(card);
            cardOutputModel.BaseType = CardConstants.TrapType.ToLower();

            cardOutputModel.Types.Add(card.CardSubCategory.First().SubCategory.Category.Name);
            cardOutputModel.Types.AddRange(card.CardSubCategory.Select(t => t.SubCategory.Name));

            return cardOutputModel;
        }
        public static CardOutputModel MapToCardOutputModel(IMapper mapper, CardSearch cardSearch)
        {
            var card = MapToTrapCard(cardSearch);
            var cardOutputModel = mapper.Map<CardOutputModel>(card);
            cardOutputModel.BaseType = CardConstants.TrapType.ToLower();

            cardOutputModel.Types.Add(card.CardSubCategory.First().SubCategory.Category.Name);
            cardOutputModel.Types.AddRange(card.CardSubCategory.Select(t => t.SubCategory.Name));

            return cardOutputModel;
        }
        public static CardOutputModel MapToCardOutputModel(IMapper mapper, BanlistCardSearch cardSearch)
        {
            var card = MapToTrapCard(cardSearch);
            var cardOutputModel = mapper.Map<CardOutputModel>(card);
            cardOutputModel.BaseType = CardConstants.TrapType.ToLower();

            cardOutputModel.Types.Add(card.CardSubCategory.First().SubCategory.Category.Name);
            cardOutputModel.Types.AddRange(card.CardSubCategory.Select(t => t.SubCategory.Name));

            return cardOutputModel;
        }
    }
}