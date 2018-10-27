using System.Collections.Generic;

namespace dueltank.application.Models.Archetypes.Output
{
    public class ArchetypeSearchResultOutputModel
    {
        public int TotalArchetypes { get; set; }
        public List<ArchetypeSearchOutputModel> Archetypes { get; set; }
    }
}