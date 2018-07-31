using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;

namespace dueltank.core.Services
{
    public interface IFormatService
    {
        Task<IList<Format>> AllFormats();
    }
}