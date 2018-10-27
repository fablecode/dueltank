namespace dueltank.core.Models.Search.Archetypes
{
    public sealed class ArchetypeSearchCriteria
    {
        public string SearchTerm { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; }
    }
}