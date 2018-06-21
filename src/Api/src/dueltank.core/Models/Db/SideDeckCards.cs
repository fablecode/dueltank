namespace dueltank.core.Models.Db
{
    public class SideDeckCards
    {
        public long SideDeckId { get; set; }
        public long CardId { get; set; }
        public int Quantity { get; set; }
        public int SortOrder { get; set; }

        public Card Card { get; set; }
        public SideDeck SideDeck { get; set; }
    }
}