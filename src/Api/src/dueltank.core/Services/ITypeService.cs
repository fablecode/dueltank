using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;

namespace dueltank.core.Services
{
    public interface ITypeService
    {
        Task<IList<Type>> AllTypes();
    }
}