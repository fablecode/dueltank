using System.Collections.Generic;

namespace dueltank.core.Models.Cards
{
    public class CardModel
    {
        public CardModel()
        {
            Types = new List<string>();
        }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CardNumber { get; set; }

        public long Id { get; set; }

        public string Limit { get; set; }

        public long? Atk { get; set; }

        public long? Def { get; set; }
        public List<string> Types { get; set; }
        public string BaseType { get; set; }
    }
}