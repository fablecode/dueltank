using System.IO;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Helpers;
using MediatR;
using Microsoft.Extensions.Options;

namespace dueltank.application.Queries.ArchetypeImageById
{
    public class ArchetypeImageByIdQueryHandler : IRequestHandler<ArchetypeImageByIdQuery, ArchetypeImageByIdQueryResult>
    {
        private readonly IOptions<ApplicationSettings> _settings;

        public ArchetypeImageByIdQueryHandler(IOptions<ApplicationSettings> settings)
        {
            _settings = settings;
        }

        public Task<ArchetypeImageByIdQueryResult> Handle(ArchetypeImageByIdQuery request, CancellationToken cancellationToken)
        {
            const string defaultImage = "no-archetype-image.jpg";

            var imageFilePath = ImageHelper.GetImagePath(request.ArchetypeId.ToString(), _settings.Value.ArchetypeImageFolderPath, defaultImage);

            var response = new ArchetypeImageByIdQueryResult
            {
                Image = File.ReadAllBytes(imageFilePath),
                ContentType = MimeMapping.MimeUtility.GetMimeMapping(Path.GetExtension(imageFilePath))
            };

            return Task.FromResult(response);
        }
    }
}