using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Models.Archetypes.Output;
using dueltank.application.Models.Decks.Output;
using dueltank.core.Models.Search.Decks;
using dueltank.core.Services;
using MediatR;

namespace dueltank.application.Queries.DeckSearch
{
    public class DeckSearchQueryHandler : IRequestHandler<DeckSearchQuery, DeckSearchResultOutputModel>
    {
        private readonly IDeckService _deckService;

        public DeckSearchQueryHandler(IDeckService deckService)
        {
            _deckService = deckService;
        }

        public async Task<DeckSearchResultOutputModel> Handle(DeckSearchQuery request, CancellationToken cancellationToken)
        {
            var response = new DeckSearchResultOutputModel();

            var result = await _deckService.Search(new DeckSearchCriteria
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                SearchTerm = request.SearchTerm
            });

            if (result.Decks.Any())
            {
                response.Decks = result.Decks.Select(DeckDetailOutputModel.From).ToList();
                response.TotalDecks = result.TotalRecords;
            }

            return response;
        }
    }
}