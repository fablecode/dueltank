namespace dueltank.application.Models.Decks.Input
{
    public sealed class DecksByUsernameInputModel
    {
        public string Username { get; set; }

        public string SearchTerm { get; set; }

        public int PageSize { get; set; } = 10;

        public int PageIndex { get; set; } = 1;
    }
}