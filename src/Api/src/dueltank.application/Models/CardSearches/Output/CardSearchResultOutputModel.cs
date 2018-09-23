using System.Collections.Generic;
using dueltank.application.Models.Cards.Output;

namespace dueltank.application.Models.CardSearches.Output
{
    public class CardSearchResultOutputModel
    {
        public int TotalRecords { get; set; }
        public List<CardOutputModel> Cards { get; set; }
    }
}