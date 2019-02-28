using System.Drawing;
using System.IO;
using dueltank.core.Models.DeckDetails;
using dueltank.Domain.SystemIO;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;

namespace dueltank.infrastructure.SystemIO
{
    public class DeckFileSystem : IDeckFileSystem
    {
        public long SaveDeckThumbnail(DeckThumbnail deckThumbnailModel)
        {
            const int quality = 90;
            var format = new PngFormat();
            var size = new Size(170, 0);

            using (var inStream = new MemoryStream(deckThumbnailModel.Thumbnail))
            {
                using (var imageFactory = new ImageFactory())
                {
                    // Load, resize, set the format and quality and save an image.
                    imageFactory
                        .Load(inStream)
                        .Resize(size)
                        .Format(format)
                        .Quality(quality)
                        .Save(deckThumbnailModel.ImageFilePath);
                }
            }

            return deckThumbnailModel.DeckId;
        }
    }
}