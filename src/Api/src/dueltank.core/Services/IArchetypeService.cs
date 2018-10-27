using System.Threading.Tasks;
using dueltank.core.Models.Archetypes;
using dueltank.core.Models.Search.Archetypes;

namespace dueltank.core.Services
{
    public interface IArchetypeService
    {
        Task<ArchetypeSearchResult> Search(ArchetypeSearchCriteria searchCriteria);
        Task<ArchetypeByIdResult> ArchetypeById(long archetypeId);
    }
}