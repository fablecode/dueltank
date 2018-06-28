using dueltank.application.Models.Decks.Output;
using MediatR;

namespace dueltank.application.Queries.DeckById
{
    public class DeckByIdQuery : IRequest<DeckDetailOutputModel>
    {
        public long DeckId { get; set; }
    }
}