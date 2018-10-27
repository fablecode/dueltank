using System.Threading.Tasks;
using dueltank.core.Models.Search.Banlists;
using dueltank.core.Services;
using dueltank.Domain.Repository;

namespace dueltank.Domain.Service
{
    public class BanlistService : IBanlistService
    {
        private readonly IBanlistRepository _banlistRepository;

        public BanlistService(IBanlistRepository banlistRepository)
        {
            _banlistRepository = banlistRepository;
        }

        public Task<BanlistCardSearchResult> LatestBanlistByFormatAcronym(string formatAcronym)
        {
            return _banlistRepository.LatestBanlistByFormatAcronym(formatAcronym);
        }
    }
}