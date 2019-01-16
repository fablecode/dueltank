using System.Collections.Generic;
using dueltank.application.Models.Cards.Output;

namespace dueltank.application.Models.Banlists.Output
{
    public sealed class LatestBanlistOutputModel
    {
        public string Format { get; set; }

        public string ReleaseDate { get; set; }
        public List<CardOutputModel> Forbidden { get; set; }
        public List<CardOutputModel> Limited { get; set; }
        public List<CardOutputModel> SemiLimited { get; set; }
        public List<CardOutputModel> Unlimited { get; set; }
    }
}