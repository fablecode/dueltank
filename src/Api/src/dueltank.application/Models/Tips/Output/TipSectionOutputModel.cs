using System.Collections.Generic;

namespace dueltank.application.Models.Tips.Output
{
    public class TipSectionOutputModel
    {
        public long CardId { get; set; }
        public string Name { get; set; }

        public List<TipOutputModel> Tips { get; set; } = new List<TipOutputModel>();
    }
}