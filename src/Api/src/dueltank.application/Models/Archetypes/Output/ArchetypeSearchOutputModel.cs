using System;
using System.Collections.Generic;
using dueltank.application.Models.Cards.Output;
using dueltank.core.Models.Archetypes;
using dueltank.core.Models.Db;

namespace dueltank.application.Models.Archetypes.Output
{
    public class ArchetypeSearchOutputModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime Updated { get; set; }

        public int TotalCards { get; set; }

        public string ThumbnailUrl { get; set; }

        public List<CardOutputModel> Cards { get; set; }

        public static ArchetypeSearchOutputModel From(Archetype entity)
        {
            return new ArchetypeSearchOutputModel
            {
                Id = entity.Id,
                Name = entity.Name,
                ThumbnailUrl = GetThumbnailUrl(entity.Id),
                Updated = entity.Updated
            };
        }

        public static ArchetypeSearchOutputModel From(ArchetypeSearch entity)
        {
            return new ArchetypeSearchOutputModel
            {
                Id = entity.Id,
                Name = entity.Name,
                ThumbnailUrl = GetThumbnailUrl(entity.Id),
                Updated = entity.Updated,
                TotalCards = entity.TotalCards
            };
        }

        private static string GetThumbnailUrl(long archetypeNumber)
        {
            return $"/api/images/archetypes/{archetypeNumber}";
        }
    }
}