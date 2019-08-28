using System;
using dueltank.application.Models.Archetypes.Output;
using MediatR;

namespace dueltank.application.Queries.ArchetypeSearch
{
    public class ArchetypeSearchQuery : IRequest<ArchetypeSearchResultOutputModel>
    {
        public string SearchTerm { get; set; }
        public int PageSize { get; set; } = 8;
        public int PageIndex { get; set; } = 1;
    }
}