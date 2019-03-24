using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using dueltank.application.Helpers;
using dueltank.application.Models.Banlists.Output;
using dueltank.core.Constants;
using dueltank.core.Services;
using MediatR;

namespace dueltank.application.Queries.LatestBanlist
{
    public class LatestBanlistQueryHandler : IRequestHandler<LatestBanlistQuery, LatestBanlistOutputModel>
    {
        private readonly IBanlistService _banlistService;
        private readonly IMapper _mapper;

        public LatestBanlistQueryHandler(IBanlistService banlistService, IMapper mapper)
        {
            _banlistService = banlistService;
            _mapper = mapper;
        }

        public async Task<LatestBanlistOutputModel> Handle(LatestBanlistQuery request, CancellationToken cancellationToken)
        {
            var response = new LatestBanlistOutputModel();

            var banlistCardSearchResult = await _banlistService.LatestBanlistByFormatAcronym(request.Format.ToString());

            if (banlistCardSearchResult.Cards.Any())
            {
                var groupedCards =
                    banlistCardSearchResult
                        .Cards
                        .GroupBy(c => c.Limit)
                        .Select(nc => nc)
                        .ToList();

                var forbiddenCards = groupedCards.SingleOrDefault(grp => grp.Key == LimitConstants.Forbidden);
                var limnitCards = groupedCards.SingleOrDefault(grp => grp.Key == LimitConstants.Limited);
                var semiLimitedCards = groupedCards.SingleOrDefault(grp => grp.Key == LimitConstants.SemiLimited);
                var unlimitedCards = groupedCards.SingleOrDefault(grp => grp.Key == LimitConstants.Unlimited);

                response.Format = request.Format.ToString().ToUpper();
                response.ReleaseDate = banlistCardSearchResult.ReleaseDate.ToString(BanlistConstants.ReleaseDateFormat);

                if (forbiddenCards != null)
                    response.Forbidden = forbiddenCards.Select(card => CardSearchMapperHelper.MapToCardOutputModel(_mapper, card)).ToList();

                if (limnitCards != null)
                    response.Limited = limnitCards.Select(card => CardSearchMapperHelper.MapToCardOutputModel(_mapper, card)).ToList();

                if (semiLimitedCards != null)
                    response.SemiLimited = semiLimitedCards.Select(card => CardSearchMapperHelper.MapToCardOutputModel(_mapper, card)).ToList();

                if (unlimitedCards != null)
                    response.Unlimited = unlimitedCards.Select(card => CardSearchMapperHelper.MapToCardOutputModel(_mapper, card)).ToList();
            }

            return response;
        }
    }
}