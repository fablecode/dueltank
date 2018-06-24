using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;

namespace dueltank.Domain.Repository
{
    public interface IDeckTypeRepository
    {
        Task<IEnumerable<DeckType>> AllDeckTypes();
    }
}