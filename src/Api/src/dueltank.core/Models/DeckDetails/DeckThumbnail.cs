namespace dueltank.core.Models.DeckDetails
{
    public class DeckThumbnail
    {
        public long DeckId { get; set; }
        public string UserId { get; set; }
        public byte[] Thumbnail { get; set; }
        public string ImageFilePath { get; set; }
    }
}