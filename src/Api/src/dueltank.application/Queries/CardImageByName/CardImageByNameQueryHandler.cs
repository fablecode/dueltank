using dueltank.core.Services;
using MediatR;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Configuration;
using dueltank.Domain.Helpers;

namespace dueltank.application.Queries.CardImageByName
{
    public class CardImageByNameQueryHandler : IRequestHandler<CardImageByNameQuery, CardImageByNameResult>
    {
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly IImageService _imageService;
        private readonly IFileService _fileService;

        public CardImageByNameQueryHandler(IOptions<ApplicationSettings> settings, IImageService imageService, IFileService fileService)
        {
            _settings = settings;
            _imageService = imageService;
            _fileService = fileService;
        }


        public Task<CardImageByNameResult> Handle(CardImageByNameQuery request, CancellationToken cancellationToken)
        {
            const string defaultImage = "no-card-image.png";

            var imageFilePath = _imageService.GetImagePath(request.Name.SanitizeFileName(), _settings.Value.CardImageFolderPath, defaultImage);

            var response = new CardImageByNameResult
            {
                Image = _fileService.ReadAllBytes(imageFilePath),
                ContentType = MimeMapping.MimeUtility.GetMimeMapping(Path.GetExtension(imageFilePath))
            };

            return Task.FromResult(response);
        }
    }
}