using System.Collections.Generic;
using dueltank.core.Models.Cards;
using dueltank.core.Models.Db;

namespace dueltank.core.Models.Archetypes
{
    public class ArchetypeByIdResult
    {
        public Archetype Archetype { get; set; }

        public List<CardSearch> Cards { get; set; }
    }
}