using System.Threading.Tasks;
using dueltank.core.Models.Search.Banlist;

namespace dueltank.Domain.Repository
{
    public interface IBanlistRepository
    {
        Task<BanlistCardSearchResult> LatestBanlistByFormatAcronym(string formatAcronym);
    }
}