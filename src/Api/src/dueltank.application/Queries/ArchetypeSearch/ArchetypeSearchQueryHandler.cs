using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Models.Archetypes.Output;
using dueltank.core.Models.Search.Archetypes;
using dueltank.core.Services;
using MediatR;

namespace dueltank.application.Queries.ArchetypeSearch
{
    public class ArchetypeSearchQueryHandler : IRequestHandler<ArchetypeSearchQuery, ArchetypeSearchResultOutputModel>
    {
        private readonly IArchetypeService _archetypeService;

        public ArchetypeSearchQueryHandler(IArchetypeService archetypeService)
        {
            _archetypeService = archetypeService;
        }

        public async Task<ArchetypeSearchResultOutputModel> Handle(ArchetypeSearchQuery request, CancellationToken cancellationToken)
        {
            var response = new ArchetypeSearchResultOutputModel();

            var result = await _archetypeService.Search(new ArchetypeSearchCriteria
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                SearchTerm = request.SearchTerm
            });

            if (result.Archetypes.Any())
            {
                response.Archetypes = result.Archetypes.Select(ArchetypeSearchOutputModel.From).ToList();
            }

            return response;
        }
    }
}