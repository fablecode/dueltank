using System;
using System.Collections.Generic;
using dueltank.core.Models.Cards;

namespace dueltank.core.Models.Search.Banlist
{
    public class BanlistCardSearchResult
    {
        public List<BanlistCardSearch> Cards { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}