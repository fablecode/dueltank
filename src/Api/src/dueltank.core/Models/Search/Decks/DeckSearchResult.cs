using System.Collections.Generic;
using dueltank.core.Models.DeckDetails;

namespace dueltank.core.Models.Search.Decks
{
    public class DeckSearchResult
    {
        public int TotalRecords { get; set; }
        public List<DeckDetail> Decks { get; set; }
    }
}