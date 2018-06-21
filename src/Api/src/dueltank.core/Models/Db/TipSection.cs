using System;

namespace dueltank.core.Models.Db
{
    public class TipSection
    {
        public long Id { get; set; }
        public long CardId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}