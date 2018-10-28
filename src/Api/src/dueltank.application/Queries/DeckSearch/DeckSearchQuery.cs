using dueltank.application.Models.Decks.Output;
using MediatR;

namespace dueltank.application.Queries.DeckSearch
{
    public class DeckSearchQuery : IRequest<DeckSearchResultOutputModel>
    {
        public string SearchTerm { get; set; }
        public int PageSize { get; set; } = 8;
        public int PageIndex { get; set; } = 1;
    }
}