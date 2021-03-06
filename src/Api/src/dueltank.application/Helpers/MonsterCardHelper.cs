﻿using System;
using System.Linq;
using AutoMapper;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Constants;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;
using dueltank.core.Models.Search.Banlists;
using Attribute = dueltank.core.Models.Db.Attribute;
using Type = dueltank.core.Models.Db.Type;

namespace dueltank.application.Helpers
{
    public static class MonsterCardHelper
    {
        public static bool IsMonsterCard(DeckCardDetail deckCard)
        {
            return IsMonsterCard(deckCard.Category);
        }
        public static bool IsMonsterCard(CardSearch cardSearch)
        {
            return IsMonsterCard(cardSearch.Category);
        }
        public static bool IsMonsterCard(BanlistCardSearch banlistCardSearch)
        {
            return IsMonsterCard(banlistCardSearch.Category);

        }
        public static bool IsMonsterCard(string category)
        {
            return string.Equals(category, CardConstants.MonsterType, StringComparison.OrdinalIgnoreCase);
        }

        public static CardDetailOutputModel MapToCardOutputModel(IMapper mapper, DeckCardDetail deckCardSearch)
        {
            var card = MapToMonsterCard(deckCardSearch);
            var cardOutputModel = mapper.Map<CardDetailOutputModel>(card);
            cardOutputModel.BaseType = BaseType(card);

            cardOutputModel.Types.Add(card.CardSubCategory.First().SubCategory.Category.Name);
            cardOutputModel.Types.AddRange(card.CardSubCategory.Select(t => t.SubCategory.Name));

            return cardOutputModel;
        }
        public static CardOutputModel MapToCardOutputModel(IMapper mapper, CardSearch cardSearch)
        {
            var card = MapToMonsterCard(cardSearch);
            var cardOutputModel = mapper.Map<CardOutputModel>(card); ;
            cardOutputModel.BaseType = BaseType(card);

            cardOutputModel.Types.Add(card.CardSubCategory.First().SubCategory.Category.Name);
            cardOutputModel.Types.AddRange(card.CardSubCategory.Select(t => t.SubCategory.Name));

            return cardOutputModel;
        }
        public static CardOutputModel MapToCardOutputModel(IMapper mapper, BanlistCardSearch cardSearch)
        {
            var card = MapToMonsterCard(cardSearch);
            var cardOutputModel = mapper.Map<CardOutputModel>(card); ;
            cardOutputModel.BaseType = BaseType(card);

            cardOutputModel.Types.Add(card.CardSubCategory.First().SubCategory.Category.Name);
            cardOutputModel.Types.AddRange(card.CardSubCategory.Select(t => t.SubCategory.Name));

            return cardOutputModel;
        }

        public static Card MapToMonsterCard(DeckCardDetail model)
        {

            var card = new Card
            {
                Id = model.Id,
                CardNumber = model.CardNumber,
                Name = model.Name,
                Description = model.Description,
                CardLevel = model.CardLevel,
                CardRank = model.CardRank,
                Atk = model.Atk,
                Def = model.Def,
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

            if (model.AttributeId > 0)
                card.CardAttribute.Add(new CardAttribute { Attribute = new Attribute { Id = model.AttributeId.Value, Name = model.Attribute } });

            if (model.TypeId > 0)
                card.CardType.Add(new CardType{ Type = new Type { Id = model.TypeId.Value, Name = model.Type } });

            return card;
        }
        public static Card MapToMonsterCard(CardSearch model)
        {
            var card = new Card
            {
                Id = model.Id,
                CardNumber = model.CardNumber,
                Name = model.Name,
                Description = model.Description,
                CardLevel = model.CardLevel,
                CardRank = model.CardRank,
                Atk = model.Atk,
                Def = model.Def,
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

            if (model.AttributeId > 0)
                card.CardAttribute.Add(new CardAttribute { Attribute = new Attribute { Id = model.AttributeId.Value, Name = model.Attribute } });

            if (model.TypeId > 0)
                card.CardType.Add(new CardType{ Type = new Type { Id = model.TypeId.Value, Name = model.Type } });

            return card;
        }
        public static Card MapToMonsterCard(BanlistCardSearch model)
        {
            var card = new Card
            {
                Id = model.Id,
                CardNumber = model.CardNumber,
                Name = model.Name,
                Description = model.Description,
                CardLevel = model.CardLevel,
                CardRank = model.CardRank,
                Atk = model.Atk,
                Def = model.Def,
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

            if (model.AttributeId > 0)
                card.CardAttribute.Add(new CardAttribute { Attribute = new Attribute { Id = model.AttributeId.Value, Name = model.Attribute } });

            if (model.TypeId > 0)
                card.CardType.Add(new CardType{ Type = new Type { Id = model.TypeId.Value, Name = model.Type } });

            return card;
        }

        public static string BaseType(Card card)
        {
            var types = card.CardSubCategory.Select(t => t.SubCategory.Name).ToArray();

            if (types.Contains(CardConstants.FusionType, StringComparer.OrdinalIgnoreCase))
                return CardConstants.FusionType.ToLower();

            if (types.Contains(CardConstants.XyzType, StringComparer.OrdinalIgnoreCase))
                return CardConstants.XyzType.ToLower();

            if (types.Contains(CardConstants.SynchronType, StringComparer.OrdinalIgnoreCase))
                return CardConstants.SynchronType.ToLower();

            if (types.Contains(CardConstants.LinkType, StringComparer.OrdinalIgnoreCase))
                return CardConstants.LinkType.ToLower();

            return CardConstants.MonsterType.ToLower();
        }
    }
}