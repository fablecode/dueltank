using System.Collections.Generic;
using System.Linq;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;

namespace dueltank.application.Models.Cards.Output
{
    public class CardDetailOutputModel
    {
        public CardDetailOutputModel()
        {
            Types = new List<string>();
        }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long? CardNumber { get; set; }

        public long Id { get; set; }

        public string Limit { get; set; }

        public long? Atk { get; set; }

        public long? Def { get; set; }
        public List<string> Types { get; set; }
        public string BaseType { get; set; }

        public static CardDetailOutputModel From(CardDetail model)
        {
            return new CardDetailOutputModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                CardNumber = model.CardNumber,
                ImageUrl = $"/api/images/cards/{model.Name}",
                Atk = model.Atk,
                Def = model.Def
            };
        }

        public static CardDetailOutputModel From(Card model)
        {
            return new CardDetailOutputModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                CardNumber = model.CardNumber,
                ImageUrl = $"/api/images/cards/{model.Name}",
                Limit = model.BanlistCard.Any() ? model.BanlistCard.First().Limit.Name.ToLower() : "unlimited",
                Atk = model.Atk,
                Def = model.Def
            };
        }
    }
}