using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Models.Decks.Output;
using dueltank.core.Models.Db;
using dueltank.core.Services;
using dueltank.Domain.Helpers;
using MediatR;

namespace dueltank.application.Queries.DeckById
{
    public class DeckByIdQueryHandler : IRequestHandler<DeckByIdQuery, DeckDetailOutputModel>
    {
        private readonly IDeckService _deckService;

        public DeckByIdQueryHandler(IDeckService deckService)
        {
            _deckService = deckService;
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
                var mainList = deckResult.MainDeck.SelectMany(c => Enumerable.Repeat(c, c.Quantity)).ToList();
                var extraList = deckResult.ExtraDeck.SelectMany(c => Enumerable.Repeat(c, c.Quantity)).ToList();
                var sideList = deckResult.SideDeck.SelectMany(c => Enumerable.Repeat(c, c.Quantity)).ToList();

                // we map to cardoutputmodel
                //response.MainDeck = mainList.Select(_manageCardMapper.MapToCardOutputModel).ToList();
                //response.ExtraDeck = extraList.Select(_manageCardMapper.MapToCardOutputModel).ToList();
                //response.SideDeck = sideList.Select(_manageCardMapper.MapToCardOutputModel).ToList();
            }

            return null;
        }
    }
}