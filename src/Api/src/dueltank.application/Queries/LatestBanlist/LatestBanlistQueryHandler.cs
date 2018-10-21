using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Helpers;
using dueltank.application.Models.Banlists.Output;
using dueltank.core.Services;
using MediatR;

namespace dueltank.application.Queries.LatestBanlist
{
    public class LatestBanlistQueryHandler : IRequestHandler<LatestBanlistQuery, LatestBanlistOutputModel>
    {
        private readonly IBanlistService _banlistService;

        public LatestBanlistQueryHandler(IBanlistService banlistService)
        {
            _banlistService = banlistService;
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

                var forbiddenCards = groupedCards.SingleOrDefault(grp => grp.Key == "Forbidden");
                var limnitCards = groupedCards.SingleOrDefault(grp => grp.Key == "Limited");
                var semiLimitedCards = groupedCards.SingleOrDefault(grp => grp.Key == "Semi-Limited");
                var unlimitedCards = groupedCards.SingleOrDefault(grp => grp.Key == "Unlimited");

                response.Format = request.Format.ToString().ToUpper();
                response.ReleaseDate = banlistCardSearchResult.ReleaseDate.ToString("MMMM dd, yyyy");

                if (forbiddenCards != null)
                    response.Forbidden = forbiddenCards.Select(CardSearchMapperHelper.MapToCardOutputModel).ToList();

                if (limnitCards != null)
                    response.Limited = limnitCards.Select(CardSearchMapperHelper.MapToCardOutputModel).ToList();

                if (semiLimitedCards != null)
                    response.SemiLimited = semiLimitedCards.Select(CardSearchMapperHelper.MapToCardOutputModel).ToList();

                if (unlimitedCards != null)
                    response.Unlimited = unlimitedCards.Select(CardSearchMapperHelper.MapToCardOutputModel).ToList();
            }

            return response;
        }
    }
}