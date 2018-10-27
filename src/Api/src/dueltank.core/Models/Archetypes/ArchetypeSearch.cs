using System;

namespace dueltank.core.Models.Archetypes
{
    public sealed class ArchetypeSearch
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
        public int TotalCards { get; set; }
    }
}