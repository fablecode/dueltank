using System;

namespace dueltank.core.Models.Db
{
    public class Archetype
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}