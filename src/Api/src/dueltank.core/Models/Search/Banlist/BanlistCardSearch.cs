namespace dueltank.core.Models.Search.Banlist
{
    public class BanlistCardSearch
    {
        public long Id { get; set; }
        public long? CardNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CardLevel { get; set; }
        public int? CardRank { get; set; }
        public int? Atk { get; set; }
        public int? Def { get; set; }
        public long CategoryId { get; set; }
        public string Category { get; set; }
        public string SubCategories { get; set; }
        public long? AttributeId { get; set; }
        public string Attribute { get; set; }
        public long? TypeId { get; set; }
        public string Type { get; set; }

        public string Limit { get; set; }
    }
}