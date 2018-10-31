using dueltank.application.Models.Archetypes.Output;
using MediatR;

namespace dueltank.application.Queries.MostRecentArchetypes
{
    public class MostRecentArchetypesQuery : IRequest<ArchetypeSearchResultOutputModel>
    {
        public int PageSize { get; set; }
    }
}