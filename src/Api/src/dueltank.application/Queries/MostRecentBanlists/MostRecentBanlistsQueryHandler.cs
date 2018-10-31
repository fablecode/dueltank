using dueltank.core.Services;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Models.Banlists.Output;

namespace dueltank.application.Queries.MostRecentBanlists
{
    public class MostRecentBanlistsQueryHandler : IRequestHandler<MostRecentBanlistsQuery, MostRecentBanlistResultOutput>
    {
        private readonly IBanlistService _banlistService;

        public MostRecentBanlistsQueryHandler(IBanlistService banlistService)
        {
            _banlistService = banlistService;
        }

        public async Task<MostRecentBanlistResultOutput> Handle(MostRecentBanlistsQuery request, CancellationToken cancellationToken)
        {
            var response = new MostRecentBanlistResultOutput();

            var result = await _banlistService.MostRecentBanlists();

            if (result.Banlists.Any())
            {
                response.Banlists = result.Banlists.Select(MostRecentBanlistOutputModel.From).ToList();
            }

            return response;
        }
    }
}