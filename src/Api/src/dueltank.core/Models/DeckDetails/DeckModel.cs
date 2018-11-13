using System.Collections.Generic;
using dueltank.core.Models.Cards;

namespace dueltank.core.Models.DeckDetails
{
    public class DeckModel
    {
        public long? Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }


        public string VideoUrl { get; set; }

        public List<CardModel> MainDeck { get; set; }

        public List<CardModel> ExtraDeck { get; set; }

        public List<CardModel> SideDeck { get; set; }
    }
}