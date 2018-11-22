namespace dueltank.application.Models.Decks.Input
{
    public sealed class DecksByUserIdInputModel
    {
        public string UserId { get; set; }

        public string SearchTerm { get; set; }

        public int PageSize { get; set; } = 10;

        public int PageIndex { get; set; } = 1;
    }
}