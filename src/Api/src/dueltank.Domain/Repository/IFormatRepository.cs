using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;

namespace dueltank.Domain.Repository
{
    public interface IFormatRepository
    {
        Task<IList<Format>> AllFormats();
    }
}