using System.Collections.Generic;

namespace dueltank.core.Models.Db
{
    public class MainDeck
    {
        public MainDeck()
        {
            MainDeckCard = new HashSet<MainDeckCard>();
        }

        public long DeckId { get; set; }

        public Deck Deck { get; set; }
        public ICollection<MainDeckCard> MainDeckCard { get; set; }
    }
}