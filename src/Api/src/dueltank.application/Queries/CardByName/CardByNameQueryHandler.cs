using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Helpers;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Services;
using MediatR;

namespace dueltank.application.Queries.CardByName
{
    public class CardByNameQueryHandler : IRequestHandler<CardByNameQuery, CardOutputModel>
    {
        private readonly ICardService _cardService;

        public CardByNameQueryHandler(ICardService cardService)
        {
            _cardService = cardService;
        }
        public async Task<CardOutputModel> Handle(CardByNameQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var result = await _cardService.GetCardByName(request.Name);

                if (result != null)
                {
                    return CardSearchMapperHelper.MapToCardOutputModel(result);
                }
            }

            return null;
        }
    }
}