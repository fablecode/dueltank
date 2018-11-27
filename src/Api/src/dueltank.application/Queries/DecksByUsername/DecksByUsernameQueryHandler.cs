using dueltank.application.Models.Decks.Output;
using dueltank.core.Models.Search.Decks;
using dueltank.core.Services;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dueltank.application.Queries.DecksByUsername
{
    public class DecksByUsernameQueryHandler : IRequestHandler<DecksByUsernameQuery, DeckSearchResultOutputModel>
    {
        private readonly IDeckService _deckService;

        public DecksByUsernameQueryHandler(IDeckService deckService)
        {
            _deckService = deckService;
        }
        public async Task<DeckSearchResultOutputModel> Handle(DecksByUsernameQuery request, CancellationToken cancellationToken)
        {
            var response = new DeckSearchResultOutputModel();

            var result = await _deckService.Search(new DeckSearchByUsernameCriteria
            {
                Username = request.Username,
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