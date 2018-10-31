using dueltank.application.Models.Decks.Output;
using MediatR;

namespace dueltank.application.Queries.MostRecentDecks
{
    public class MostRecentDecksQuery : IRequest<DeckSearchResultOutputModel>
    {
        public int PageSize { get; set; }
    }
}