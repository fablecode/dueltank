﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Models.Decks.Output;
using dueltank.core.Models.Search.Decks;
using dueltank.core.Services;
using MediatR;

namespace dueltank.application.Queries.DecksByUserId
{
    public class DecksByUserIdQueryHandler : IRequestHandler<DecksByUserIdQuery, DeckSearchResultOutputModel>
    {
        private readonly IDeckService _deckService;

        public DecksByUserIdQueryHandler(IDeckService deckService)
        {
            _deckService = deckService;
        }
        public async Task<DeckSearchResultOutputModel> Handle(DecksByUserIdQuery request, CancellationToken cancellationToken)
        {
            var response = new DeckSearchResultOutputModel();

            var result = await _deckService.Search(new DeckSearchByUserIdCriteria
            {
                UserId = request.UserId,
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