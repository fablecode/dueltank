namespace dueltank.application
{
    public class ApplicationSettings
    {
        public string DeckThumbnailImageFolderPath { get; set; }
        public string CardImageFolderPath { get; set; }

    }

    public class YgoApiSettings
    {
        public string DomainUrl { get; set; }

        public string CardImageByNameUrl { get; set; }
    }
}