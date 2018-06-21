using System;

namespace dueltank.core.Models.Db
{
    public class Deck
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public AspNetUsers User { get; set; }
        public ExtraDeck ExtraDeck { get; set; }
        public MainDeck MainDeck { get; set; }
        public SideDeck SideDeck { get; set; }
    }
}