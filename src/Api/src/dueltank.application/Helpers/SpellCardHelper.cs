﻿using dueltank.application.Models.Cards.Output;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;
using System;
using System.Linq;
using AutoMapper;
using dueltank.core.Constants;
using dueltank.core.Models.Search.Banlists;

namespace dueltank.application.Helpers
{
    public static class SpellCardHelper
    {
        public static bool IsSpellCard(DeckCardDetail deckCard)
        {
            return string.Equals(deckCard.Category, CardConstants.SpellType, StringComparison.OrdinalIgnoreCase);
        }
        public static bool IsSpellCard(CardSearch card)
        {
            return string.Equals(card.Category, CardConstants.SpellType, StringComparison.OrdinalIgnoreCase);
        }
        public static bool IsSpellCard(BanlistCardSearch card)
        {
            return string.Equals(card.Category, CardConstants.SpellType, StringComparison.OrdinalIgnoreCase);
        }

        public static Card MapToSpellCard(DeckCardDetail model)
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
        public static Card MapToSpellCard(CardSearch model)
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
        public static Card MapToSpellCard(BanlistCardSearch model)
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

        public static CardDetailOutputModel MapToCardOutputModel(IMapper mapper, DeckCardDetail model)
        {
            var card = MapToSpellCard(model);
            var cardOutputModel = mapper.Map<CardDetailOutputModel>(card);
            cardOutputModel.BaseType = CardConstants.SpellType.ToLower();

            cardOutputModel.Types.Add(card.CardSubCategory.First().SubCategory.Category.Name);
            cardOutputModel.Types.AddRange(card.CardSubCategory.Select(t => t.SubCategory.Name));

            return cardOutputModel;
        }
        public static CardOutputModel MapToCardOutputModel(IMapper mapper, CardSearch model)
        {
            var card = MapToSpellCard(model);
            var cardOutputModel = mapper.Map<CardOutputModel>(card);
            cardOutputModel.BaseType = CardConstants.SpellType.ToLower();

            cardOutputModel.Types.Add(card.CardSubCategory.First().SubCategory.Category.Name);
            cardOutputModel.Types.AddRange(card.CardSubCategory.Select(t => t.SubCategory.Name));

            return cardOutputModel;
        }
        public static CardOutputModel MapToCardOutputModel(IMapper mapper, BanlistCardSearch model)
        {
            var card = MapToSpellCard(model);
            var cardOutputModel = mapper.Map<CardOutputModel>(card);
            cardOutputModel.BaseType = CardConstants.SpellType.ToLower();

            cardOutputModel.Types.Add(card.CardSubCategory.First().SubCategory.Category.Name);
            cardOutputModel.Types.AddRange(card.CardSubCategory.Select(t => t.SubCategory.Name));

            return cardOutputModel;
        }
    }
}