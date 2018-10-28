using System.Collections.Generic;

namespace dueltank.application.Models.Decks.Output
{
    public class DeckSearchResultOutputModel
    {
        public long TotalDecks { get; set; }

        public List<DeckDetailOutputModel> Decks { get; set; }
    }
}