using System;

namespace dueltank.core.Models.Db
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}