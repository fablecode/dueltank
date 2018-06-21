using System.Collections.Generic;

namespace dueltank.core.Models.Db
{
    public class SideDeck
    {
        public SideDeck()
        {
            SideDeckCards = new HashSet<SideDeckCards>();
        }

        public long DeckId { get; set; }

        public Deck Deck { get; set; }
        public ICollection<SideDeckCards> SideDeckCards { get; set; }
    }
}