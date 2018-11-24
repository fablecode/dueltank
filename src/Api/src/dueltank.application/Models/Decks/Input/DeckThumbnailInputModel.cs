namespace dueltank.application.Models.Decks.Input
{
    public class DeckThumbnailInputModel
    {
        public long? DeckId { get; set; }
        public string UserId { get; set; }
        public byte[] Thumbnail { get; set; }

    }
}