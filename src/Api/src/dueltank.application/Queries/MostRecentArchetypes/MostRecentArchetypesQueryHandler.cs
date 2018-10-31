using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Models.Archetypes.Output;
using dueltank.core.Services;
using MediatR;

namespace dueltank.application.Queries.MostRecentArchetypes
{
    public class MostRecentArchetypesQueryHandler : IRequestHandler<MostRecentArchetypesQuery, ArchetypeSearchResultOutputModel>
    {
        private readonly IArchetypeService _archetypeService;

        public MostRecentArchetypesQueryHandler(IArchetypeService archetypeService)
        {
            _archetypeService = archetypeService;
        }
        public async Task<ArchetypeSearchResultOutputModel> Handle(MostRecentArchetypesQuery request, CancellationToken cancellationToken)
        {
            var response = new ArchetypeSearchResultOutputModel();

            var result = await _archetypeService.MostRecentArchetypes(request.PageSize);

            if (result.Archetypes.Any())
            {
                response.Archetypes = result.Archetypes.Select(ArchetypeSearchOutputModel.From).ToList();
                response.TotalArchetypes = result.Archetypes.Count;
            }

            return response;
        }
    }
}