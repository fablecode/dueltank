using System.Threading.Tasks;
using dueltank.core.Models.Search.Banlist;

namespace dueltank.core.Services
{
    public interface IBanlistService
    {
        Task<BanlistCardSearchResult> LatestBanlistByFormatAcronym(string formatAcronym);
    }
}