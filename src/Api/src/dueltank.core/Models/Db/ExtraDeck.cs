using System.Collections.Generic;

namespace dueltank.core.Models.Db
{
    public class ExtraDeck
    {
        public ExtraDeck()
        {
            ExtraDeckCard = new HashSet<ExtraDeckCard>();
        }

        public long DeckId { get; set; }

        public Deck Deck { get; set; }
        public ICollection<ExtraDeckCard> ExtraDeckCard { get; set; }
    }
}