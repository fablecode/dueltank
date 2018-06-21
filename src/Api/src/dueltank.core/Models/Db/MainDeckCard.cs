namespace dueltank.core.Models.Db
{
    public class MainDeckCard
    {
        public long MainDeckId { get; set; }
        public long CardId { get; set; }
        public int Quantity { get; set; }
        public int SortOrder { get; set; }

        public Card Card { get; set; }
        public MainDeck MainDeck { get; set; }
    }
}