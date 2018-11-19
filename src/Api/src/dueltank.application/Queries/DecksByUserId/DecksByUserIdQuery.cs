using dueltank.application.Models.Decks.Output;
using MediatR;

namespace dueltank.application.Queries.DecksByUserId
{
    public class DecksByUserIdQuery : IRequest<DeckSearchResultOutputModel>
    {
        public string UserId { get; set; }
        public string SearchTerm { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}