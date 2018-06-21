using System;

namespace dueltank.core.Models.Db
{
    public class Format
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}