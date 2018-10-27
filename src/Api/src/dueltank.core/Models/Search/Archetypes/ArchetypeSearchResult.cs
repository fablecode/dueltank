using System.Collections.Generic;
using dueltank.core.Models.Archetypes;

namespace dueltank.core.Models.Search.Archetypes
{
    public sealed class ArchetypeSearchResult
    {
        public List<ArchetypeSearch> Archetypes { get; set; }

        public int TotalRecords { get; set; }
    }
}