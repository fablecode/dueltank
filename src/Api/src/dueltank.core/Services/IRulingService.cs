using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;

namespace dueltank.core.Services
{
    public interface IRulingService
    {
        Task<IList<RulingSection>> GetRulingsByCardId(long cardId);
    }
}