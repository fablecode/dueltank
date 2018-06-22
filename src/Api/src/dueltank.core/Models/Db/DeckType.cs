using System;
using System.Collections.Generic;

namespace dueltank.core.Models.Db
{
    public class DeckType
    {
        public DeckType()
        {
            DeckCard = new HashSet<DeckCard>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public ICollection<DeckCard> DeckCard { get; set; }
    }
}