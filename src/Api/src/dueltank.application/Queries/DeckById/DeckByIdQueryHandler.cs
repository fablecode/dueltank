using dueltank.application.Helpers;
using dueltank.application.Models.Decks.Output;
using dueltank.core.Services;
using dueltank.Domain.Helpers;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace dueltank.application.Queries.DeckById
{
    public class DeckByIdQueryHandler : IRequestHandler<DeckByIdQuery, DeckDetailOutputModel>
    {
        private readonly IDeckService _deckService;
        private readonly IMapper _mapper;

        public DeckByIdQueryHandler(IDeckService deckService, IMapper mapper)
        {
            _deckService = deckService;
            _mapper = mapper;
        }

        public async Task<DeckDetailOutputModel> Handle(DeckByIdQuery request, CancellationToken cancellationToken)
        {
            var deckResult = await _deckService.GetDeckById(request.DeckId);

            if (deckResult != null)
            {
                var response = DeckDetailOutputModel.From(deckResult);

                if (!string.IsNullOrWhiteSpace(response.YoutubeUrl))
                    response.VideoId = YoutubeHelpers.ExtractVideoId(response.YoutubeUrl);

                // we duplicate card based on quantity property, also maintain order
                var mainList = deckResult.MainDeck.SelectMany(c => Enumerable.Repeat(c, c.Quantity)).OrderBy(c => c.SortOrder).ToList();
                var extraList = deckResult.ExtraDeck.SelectMany(c => Enumerable.Repeat(c, c.Quantity)).OrderBy(c => c.SortOrder).ToList();
                var sideList = deckResult.SideDeck.SelectMany(c => Enumerable.Repeat(c, c.Quantity)).OrderBy(c => c.SortOrder).ToList();

                // we map to cardoutputmodel
                response.MainDeck = mainList.Select(card => CardMapperHelper.MapToCardOutputModel(_mapper, card)).ToList();
                response.ExtraDeck = extraList.Select(card => CardMapperHelper.MapToCardOutputModel(_mapper, card)).ToList();
                response.SideDeck = sideList.Select(card => CardMapperHelper.MapToCardOutputModel(_mapper, card)).ToList();

                return response;
            }

            return null;
        }
    }
}