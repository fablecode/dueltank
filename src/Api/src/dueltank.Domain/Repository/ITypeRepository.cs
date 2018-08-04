using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;

namespace dueltank.Domain.Repository
{
    public interface ITypeRepository
    {
        Task<IList<Type>> AllTypes();
    }
}