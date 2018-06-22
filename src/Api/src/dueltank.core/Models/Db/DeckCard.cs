namespace dueltank.core.Models.Db
{
    public class DeckCard
    {
        public long DeckTypeId { get; set; }
        public long DeckId { get; set; }
        public long CardId { get; set; }
        public int Quantity { get; set; }
        public int SortOrder { get; set; }

        public Card Card { get; set; }
        public Deck Deck { get; set; }
        public DeckType DeckType { get; set; }
    }
}