using System.Threading.Tasks;
using dueltank.core.Models.Search.Banlists;

namespace dueltank.core.Services
{
    public interface IBanlistService
    {
        Task<BanlistCardSearchResult> LatestBanlistByFormatAcronym(string formatAcronym);
        Task<MostRecentBanlistResult> MostRecentBanlists();
    }
}