using dueltank.core.Services;
using MediatR;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace dueltank.application.Queries.ArchetypeImageById
{
    public class ArchetypeImageByIdQueryHandler : IRequestHandler<ArchetypeImageByIdQuery, ArchetypeImageByIdQueryResult>
    {
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly IImageService _imageService;
        private readonly IFileService _fileService;

        public ArchetypeImageByIdQueryHandler(IOptions<ApplicationSettings> settings, IImageService imageService, IFileService fileService)
        {
            _settings = settings;
            _imageService = imageService;
            _fileService = fileService;
        }

        public Task<ArchetypeImageByIdQueryResult> Handle(ArchetypeImageByIdQuery request, CancellationToken cancellationToken)
        {
            const string defaultImage = "no-archetype-image.jpg";

            var imageFilePath = _imageService.GetImagePath(request.ArchetypeId.ToString(), _settings.Value.ArchetypeImageFolderPath, defaultImage);

            var response = new ArchetypeImageByIdQueryResult
            {
                Image = _fileService.ReadAllBytes(imageFilePath),
                ContentType = MimeMapping.MimeUtility.GetMimeMapping(Path.GetExtension(imageFilePath))
            };

            return Task.FromResult(response);
        }
    }
}