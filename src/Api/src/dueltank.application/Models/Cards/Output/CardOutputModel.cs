using System.Collections.Generic;
using System.Linq;
using dueltank.core.Models.Db;

namespace dueltank.application.Models.Cards.Output
{
    public class CardOutputModel
    {
        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CardNumber { get; set; }

        public long Id { get; set; }

        public string Limit { get; set; }

        public long? Atk { get; set; }

        public long? Def { get; set; }
        public List<string> Types { get; set; } = new List<string>();
        public string BaseType { get; set; }

        public static CardOutputModel From(Card model)
        {
            return new CardOutputModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                CardNumber = model.CardNumber?.ToString(),
                ImageUrl = $"/api/images/cards/{model.Id}",
                Limit = model.BanlistCard.Any() ? model.BanlistCard.First().Limit.Name.ToLower() : "unlimited",
                Atk = model.Atk,
                Def = model.Def
            };
        }
    }
}