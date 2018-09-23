using dueltank.application.Models.CardSearches.Output;
using MediatR;

namespace dueltank.application.Queries.CardSearches
{
    public class CardSearchQuery : IRequest<CardSearchResultOutputModel>
    {
        public long LimitId { get; set; }
        public long CategoryId { get; set; }
        public long SubCategoryId { get; set; }
        public long AttributeId { get; set; }
        public long TypeId { get; set; }
        public int LvlRank { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public string SearchTerm { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
    }
}