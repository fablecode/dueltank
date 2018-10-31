using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Models.Decks.Output;
using dueltank.core.Services;
using MediatR;

namespace dueltank.application.Queries.MostRecentDecks
{
    public class MostRecentDecksHandler : IRequestHandler<MostRecentDecksQuery, DeckSearchResultOutputModel>
    {
        private readonly IDeckService _deckService;

        public MostRecentDecksHandler(IDeckService deckService)
        {
            _deckService = deckService;
        }

        public async Task<DeckSearchResultOutputModel> Handle(MostRecentDecksQuery request, CancellationToken cancellationToken)
        {
            var response = new DeckSearchResultOutputModel();

            var result = await _deckService.MostRecentDecks(request.PageSize);

            if (result.Decks.Any())
            {
                response.Decks = result.Decks.Select(DeckDetailOutputModel.From).ToList();
                response.TotalDecks = response.Decks.Count;
            }

            return response;
        }
    }
}