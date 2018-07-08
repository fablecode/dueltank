using MediatR;

namespace dueltank.application.Queries.DeckThumbnailImagePath
{
    public class DeckThumbnailImagePathQuery : IRequest<DeckThumbnailImagePathQueryResult>
    {
        public long DeckId { get; set; }
    }
}