using dueltank.application.Helpers;
using dueltank.application.Models.CardSearches.Output;
using dueltank.core.Models.Search.Cards;
using dueltank.core.Services;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dueltank.application.Queries.CardSearches
{
    public class CardSearchQueryHandler : IRequestHandler<CardSearchQuery, CardSearchResultOutputModel>
    {
        private readonly ICardService _cardService;

        public CardSearchQueryHandler(ICardService cardService)
        {
            _cardService = cardService;
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
            response.Cards = searchResult.Cards.Select(CardSearchMapperHelper.MapToCardOutputModel).ToList();

            return response;
        }
    }
}