using dueltank.core.Services;
using MediatR;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace dueltank.application.Queries.DeckThumbnailImagePath
{
    public class DeckThumbnailImagePathQueryHandler : IRequestHandler<DeckThumbnailImagePathQuery, DeckThumbnailImagePathQueryResult>
    {
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly IImageService _imageService;
        private readonly IFileService _fileService;

        public DeckThumbnailImagePathQueryHandler(IOptions<ApplicationSettings> settings, IImageService imageService, IFileService fileService)
        {
            _settings = settings;
            _imageService = imageService;
            _fileService = fileService;
        }
        public Task<DeckThumbnailImagePathQueryResult> Handle(DeckThumbnailImagePathQuery request, CancellationToken cancellationToken)
        {
            const string defaultImage = "no-deck-thumbnail.png";

            var imageFilePath = _imageService.GetImagePath(request.DeckId.ToString(), _settings.Value.DeckThumbnailImageFolderPath, defaultImage);

            var response = new DeckThumbnailImagePathQueryResult
            {
                Image = _fileService.ReadAllBytes(imageFilePath),
                ContentType = MimeMapping.MimeUtility.GetMimeMapping(Path.GetExtension(imageFilePath))
            };

            return Task.FromResult(response);
        }
    }
}