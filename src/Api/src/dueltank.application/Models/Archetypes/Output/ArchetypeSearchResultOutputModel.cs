using System.Collections.Generic;

namespace dueltank.application.Models.Archetypes.Output
{
    public class ArchetypeSearchResultOutputModel
    {
        public int TotalArchetypes { get; set; }
        public List<ArchetypeSearchOutputModel> Archetypes { get; set; }
    }

    public class PaginationOutputModel<T>
    {
        public int TotalItems { get; set; }
        public List<T> Items { get; set; }
    }
}