using System.IO;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Helpers;
using MediatR;
using Microsoft.Extensions.Options;

namespace dueltank.application.Queries.CardImageByName
{
    public class CardImageByNameQueryHandler : IRequestHandler<CardImageByNameQuery, CardImageByNameResult>
    {
        private readonly IOptions<ApplicationSettings> _settings;

        public CardImageByNameQueryHandler(IOptions<ApplicationSettings> settings)
        {
            _settings = settings;
        }


        public Task<CardImageByNameResult> Handle(CardImageByNameQuery request, CancellationToken cancellationToken)
        {
            const string defaultImage = "no-card-image.png";

            var imageFilePath = ImageHelper.GetImagePath(request.Name, _settings.Value.CardImageFolderPath, defaultImage);

            var response = new CardImageByNameResult
            {
                Image = File.ReadAllBytes(imageFilePath),
                ContentType = MimeMapping.MimeUtility.GetMimeMapping(Path.GetExtension(imageFilePath))
            };

            return Task.FromResult(response);
        }
    }
}