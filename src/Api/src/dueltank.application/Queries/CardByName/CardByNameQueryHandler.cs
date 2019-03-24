using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using dueltank.application.Helpers;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Services;
using MediatR;

namespace dueltank.application.Queries.CardByName
{
    public class CardByNameQueryHandler : IRequestHandler<CardByNameQuery, CardOutputModel>
    {
        private readonly ICardService _cardService;
        private readonly IMapper _mapper;

        public CardByNameQueryHandler(ICardService cardService, IMapper mapper)
        {
            _cardService = cardService;
            _mapper = mapper;
        }
        public async Task<CardOutputModel> Handle(CardByNameQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var result = await _cardService.GetCardByName(request.Name);

                if (result != null)
                {
                    return CardSearchMapperHelper.MapToCardOutputModel(_mapper, result);
                }
            }

            return null;
        }
    }
}