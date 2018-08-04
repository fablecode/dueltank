using dueltank.application.Models.Banlists.Output;

namespace dueltank.application.Models.Formats.Output
{
    public class FormatOutputModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public BanlistOutputModel LatestBanlist { get; set; }
    }
}