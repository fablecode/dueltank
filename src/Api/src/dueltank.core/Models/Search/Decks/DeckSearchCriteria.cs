namespace dueltank.core.Models.Search.Decks
{
    public class DeckSearchCriteria
    {
        public string SearchTerm { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;

    }
}