using System;

namespace dueltank.core.Models.Search.Banlists
{
    public class MostRecentBanlist
    {
        public Guid Id { get; set; }
        public string Acronym { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}