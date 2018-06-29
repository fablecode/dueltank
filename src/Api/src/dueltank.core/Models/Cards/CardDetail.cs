using System;

namespace dueltank.core.Models.Cards
{
    public class CardDetail
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public string Description { get; set; }
        public int? CardLevel { get; set; }
        public int? CardRank { get; set; }
        public int? Atk { get; set; }
        public int? Def { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public long CategoryId { get; set; }
        public string Category { get; set; }
        public string SubCategories { get; set; }
        public long? AttributeId { get; set; }
        public string Attribute { get; set; }
        public long? TypeId { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public int SortOrder { get; set; }
        public string DeckType { get; set; }
    }
}