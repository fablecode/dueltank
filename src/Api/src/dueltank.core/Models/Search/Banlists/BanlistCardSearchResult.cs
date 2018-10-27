using System;
using System.Collections.Generic;

namespace dueltank.core.Models.Search.Banlists
{
    public class BanlistCardSearchResult
    {
        public List<BanlistCardSearch> Cards { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}