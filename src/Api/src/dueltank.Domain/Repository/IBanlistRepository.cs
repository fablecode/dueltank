using System.Threading.Tasks;
using dueltank.core.Models.Search.Banlists;

namespace dueltank.Domain.Repository
{
    public interface IBanlistRepository
    {
        Task<BanlistCardSearchResult> LatestBanlistByFormatAcronym(string formatAcronym);
        Task<MostRecentBanlistResult> MostRecentBanlists(int pageSize);
    }
}