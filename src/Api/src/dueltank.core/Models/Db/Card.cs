using System;
using System.Collections.Generic;

namespace dueltank.core.Models.Db
{
    public class Card
    {
        public Card()
        {
            ExtraDeckCard = new HashSet<ExtraDeckCard>();
            MainDeckCard = new HashSet<MainDeckCard>();
            SideDeckCards = new HashSet<SideDeckCards>();
        }

        public long Id { get; set; }
        public string CardNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CardLevel { get; set; }
        public int? CardRank { get; set; }
        public int? Atk { get; set; }
        public int? Def { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public ICollection<ExtraDeckCard> ExtraDeckCard { get; set; }
        public ICollection<MainDeckCard> MainDeckCard { get; set; }
        public ICollection<SideDeckCards> SideDeckCards { get; set; }
    }
}