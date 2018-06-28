using System;
using System.Collections.Generic;
using System.Linq;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;

namespace dueltank.application.Helpers
{
    public static class SpellCardHelper
    {
        public const string SpellType = "Spell";

        public static bool IsSpellCard(CardDetail card)
        {
            return string.Equals(card.Category, SpellType, StringComparison.OrdinalIgnoreCase);
        }

        public static Card MapToSpellCard(CardDetail model)
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

        public static CardDetailOutputModel MapToCardOutputModel(CardDetail model)
        {
            var card = MapToSpellCard(model);
            var cardOutputModel = CardDetailOutputModel.From(card);
            cardOutputModel.BaseType = SpellType.ToLower();

            cardOutputModel.Types.Add(card.CardSubCategory.First().SubCategory.Category.Name);
            cardOutputModel.Types.AddRange(card.CardSubCategory.Select(t => t.SubCategory.Name));

            return cardOutputModel;
        }
    }
}