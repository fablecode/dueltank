using System.Collections.Generic;
using dueltank.core.Models.Cards;

namespace dueltank.core.Models.Search.Cards
{
    public class CardSearchResult
    {
        public List<CardSearch> Cards { get; set; }
        public int TotalRecords { get; set; }
    }
}