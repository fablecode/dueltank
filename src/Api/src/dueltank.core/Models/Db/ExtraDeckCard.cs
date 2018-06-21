namespace dueltank.core.Models.Db
{
    public class ExtraDeckCard
    {
        public long ExtraDeckId { get; set; }
        public long CardId { get; set; }
        public int Quantity { get; set; }
        public int SortOrder { get; set; }

        public Card Card { get; set; }
        public ExtraDeck ExtraDeck { get; set; }
    }
}