using System;
using System.Collections.Generic;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;

namespace dueltank.application.Helpers
{
    public static class SpellCardHelper
    {
        public const string SPELL_TYPE = "Spell";

        public static bool IsSpellCard(CardDetail card)
        {
            return string.Equals(card.Category, SPELL_TYPE, StringComparison.OrdinalIgnoreCase);
        }

        public staCard MapToSpellCard(CardDetail model)
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
                card.SubCategories.Add(new SubCategory
                {
                    Name = subCategory,
                    Category = new Category { Id = model.CategoryId, Name = model.Category }
                });
            }


            return card;
        }

        public static CardDetailOutputModel MapToCardOutputModel(CardDetail model)
        {
            var subCategoryList = model.SubCategories.Split(',');

            var card = MapToSpellCard(model);
            var cardOutputModel = CardDetailOutputModel.From(card);
            cardOutputModel.BaseType = SPELL_TYPE.ToLower();

            cardOutputModel.Types.Add(card.SubCategories.First().Category.Name);
            cardOutputModel.Types.AddRange(card.SubCategories.Select(t => t.Name));

            return cardOutputModel;
        }

        private static IEnumerable<SubCategory> MapToSubCategories(string subCategories)
        {
            var subCategoryList = new List<SubCategory>();

            foreach (var subCategory in subCategories.Split(','))
            {
                subCategoryList.Add(new SubCategory
                {
                    Name = subCategory,
                    Category = new Category { Id = model.CategoryId, Name = model.Category }
                });

            }
        }
    }
}