using System;
using System.Linq;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Constants;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;

namespace dueltank.application.Helpers
{
    public static class TrapCardHelper
    {
        public static bool IsTrapCard(CardDetail card)
        {
            return string.Equals(card.Category, CardConstants.TrapType, StringComparison.OrdinalIgnoreCase);
        }

        public static Card MapToTrapCard(CardDetail model)
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

        public static CardDetailOutputModel MapToCardOutputModel(CardDetail cardSearch)
        {
            var card = MapToTrapCard(cardSearch);
            var cardOutputModel = CardDetailOutputModel.From(card);
            cardOutputModel.BaseType = CardConstants.TrapType.ToLower();

            cardOutputModel.Types.Add(card.CardSubCategory.First().SubCategory.Category.Name);
            cardOutputModel.Types.AddRange(card.CardSubCategory.Select(t => t.SubCategory.Name));

            return cardOutputModel;
        }
    }
}