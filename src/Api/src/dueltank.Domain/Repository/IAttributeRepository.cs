using System.Collections.Generic;
using System.Threading.Tasks;
using dueltank.core.Models.Db;

namespace dueltank.Domain.Repository
{
    public interface IAttributeRepository
    {
        Task<IList<Attribute>> AllAttributes();
    }
}