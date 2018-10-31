using System.Threading.Tasks;
using dueltank.core.Models.Archetypes;
using dueltank.core.Models.Search.Archetypes;

namespace dueltank.Domain.Repository
{
    public interface IArchetypeRepository
    {
        Task<ArchetypeSearchResult> Search(ArchetypeSearchCriteria searchCriteria);
        Task<ArchetypeByIdResult> ArchetypeById(long archetypeId);
        Task<MostRecentArchetypesResult> MostRecentArchetypes(int pageSize);
    }
}