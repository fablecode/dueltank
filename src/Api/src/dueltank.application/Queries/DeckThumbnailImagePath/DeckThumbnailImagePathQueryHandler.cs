using System.IO;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Helpers;
using MediatR;
using Microsoft.Extensions.Options;

namespace dueltank.application.Queries.DeckThumbnailImagePath
{
    public class DeckThumbnailImagePathQueryHandler : IRequestHandler<DeckThumbnailImagePathQuery, DeckThumbnailImagePathQueryResult>
    {
        private readonly IOptions<ApplicationSettings> _settings;

        public DeckThumbnailImagePathQueryHandler(IOptions<ApplicationSettings> settings)
        {
            _settings = settings;
        }
        public Task<DeckThumbnailImagePathQueryResult> Handle(DeckThumbnailImagePathQuery request, CancellationToken cancellationToken)
        {
            const string defaultImage = "no-deck-thumbnail.png";

            var imageFilePath = ImageHelper.GetImagePath(request.DeckId.ToString(), _settings.Value.DeckThumbnailImageFolderPath, defaultImage);

            var response = new DeckThumbnailImagePathQueryResult
            {
                Image = File.ReadAllBytes(imageFilePath),
                ContentType = MimeMapping.MimeUtility.GetMimeMapping(Path.GetExtension(imageFilePath))
            };

            return Task.FromResult(response);

        }
    }
}