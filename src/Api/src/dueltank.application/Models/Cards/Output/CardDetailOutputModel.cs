using System.Collections.Generic;

namespace dueltank.application.Models.Cards.Output
{
    public class CardDetailOutputModel
    {
        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long? CardNumber { get; set; }

        public long Id { get; set; }

        public string Limit { get; set; }

        public long? Atk { get; set; }

        public long? Def { get; set; }
        public List<string> Types { get; set; } = new List<string>();
        public string BaseType { get; set; }
    }
}