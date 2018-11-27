using dueltank.application.Models.Decks.Output;
using MediatR;

namespace dueltank.application.Queries.DecksByUsername
{
    public class DecksByUsernameQuery : IRequest<DeckSearchResultOutputModel>
    {
        public string Username { get; set; }
        public string SearchTerm { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}