using System;
using System.Collections.Generic;

namespace dueltank.core.Models.Db
{
    public class Deck
    {
        public Deck()
        {
            DeckCard = new HashSet<DeckCard>();
        }

        public long Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public AspNetUsers User { get; set; }
        public ICollection<DeckCard> DeckCard { get; set; }
    }
}