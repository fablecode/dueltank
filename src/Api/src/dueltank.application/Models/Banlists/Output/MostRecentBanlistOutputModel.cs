using dueltank.core.Models.Search.Banlists;

namespace dueltank.application.Models.Banlists.Output
{
    public class MostRecentBanlistOutputModel
    {
        public string Acronym { get; set; }
        public string Name { get; set; }
        public string ReleaseDate { get; set; }

        public static MostRecentBanlistOutputModel From(MostRecentBanlist entity)
        {
            return new MostRecentBanlistOutputModel
            {
                Acronym = entity.Acronym,
                Name = entity.Name,
                ReleaseDate = entity.ReleaseDate.ToString("MMMM dd, yyyy")
            };
        }
    }
}