using System.Threading.Tasks;
using dueltank.core.Models.Archetypes;
using dueltank.core.Models.Search.Archetypes;
using dueltank.core.Services;
using dueltank.Domain.Repository;

namespace dueltank.Domain.Service
{
    public class ArchetypeService : IArchetypeService
    {
        private readonly IArchetypeRepository _archetypeRepository;

        public ArchetypeService(IArchetypeRepository archetypeRepository)
        {
            _archetypeRepository = archetypeRepository;
        }

        public Task<ArchetypeSearchResult> Search(ArchetypeSearchCriteria searchCriteria)
        {
            return _archetypeRepository.Search(searchCriteria);
        }

        public Task<ArchetypeByIdResult> ArchetypeById(long archetypeId)
        {
            return _archetypeRepository.ArchetypeById(archetypeId);
        }

    }
}