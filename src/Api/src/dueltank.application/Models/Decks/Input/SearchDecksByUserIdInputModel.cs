namespace dueltank.application.Models.Decks.Input
{
    public sealed class SearchDecksByUserIdInputModel
    {
        public string SearchTerm { get; set; }

        public int PageSize { get; set; } = 10;

        public int PageIndex { get; set; } = 1;
    }
}