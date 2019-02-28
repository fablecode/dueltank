using dueltank.core.Models.DeckDetails;

namespace dueltank.Domain.SystemIO
{
    public interface IDeckFileSystem
    {
        long SaveDeckThumbnail(DeckThumbnail deckThumbnailModel);
    }
}