using System;

namespace dueltank.core.Models.Db
{
    public class Banlist
    {
        public long Id { get; set; }
        public long FormatId { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}