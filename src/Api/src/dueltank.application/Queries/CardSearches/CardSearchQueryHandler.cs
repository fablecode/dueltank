using dueltank.application.Helpers;
using dueltank.application.Models.CardSearches.Output;
using dueltank.core.Models.Search.Cards;
using dueltank.core.Services;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace dueltank.application.Queries.CardSearches
{
    public class CardSearchQueryHandler : IRequestHandler<CardSearchQuery, CardSearchResultOutputModel>
    {
        private readonly ICardService _cardService;
        private readonly IMapper _mapper;

        public CardSearchQueryHandler(ICardService cardService, IMapper mapper)
        {
            _cardService = cardService;
            _mapper = mapper;
        }

        public async Task<CardSearchResultOutputModel> Handle(CardSearchQuery request, CancellationToken cancellationToken)
        {
            var response = new CardSearchResultOutputModel();

            var searchCriteria = new CardSearchCriteria
            {
                BanlistId = request.BanlistId,
                LimitId = request.LimitId,
                CategoryId = request.CategoryId,
                SubCategoryId = request.SubCategoryId,
                AttributeId = request.AttributeId,
                TypeId = request.TypeId,
                LvlRank = request.LvlRank,
                Atk = request.Atk,
                Def = request.Def,
                SearchTerm = request.SearchTerm,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex
            };

            var searchResult = await _cardService.Search(searchCriteria);

            response.TotalRecords = searchResult.TotalRecords;
            response.Cards = searchResult.Cards.Select(card => CardSearchMapperHelper.MapToCardOutputModel(_mapper, card)).ToList();

            return response;
        }
    }
}