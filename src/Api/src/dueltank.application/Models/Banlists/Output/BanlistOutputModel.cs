using dueltank.application.Models.BanlistCards.Output;
using System;
using System.Collections.Generic;

namespace dueltank.application.Models.Banlists.Output
{
    public class BanlistOutputModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long FormatId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IEnumerable<BanlistCardOutputModel> Cards { get; set; }
    }
}