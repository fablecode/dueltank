using System.Collections.Generic;
using System.Linq;
using dueltank.application.Models.BanlistCards.Output;
using dueltank.application.Models.Banlists.Output;
using dueltank.application.Models.Formats.Output;
using dueltank.core.Models.Db;

namespace dueltank.application.Helpers
{
    public static class FormatMapper
    {
        public static IEnumerable<FormatOutputModel> MapToOutputModels(this IList<Format> formats)
        {
            if(formats == null || !formats.Any())
                return new FormatOutputModel[0];

            return formats.Select(format =>
            {
                var model = new FormatOutputModel
                {
                    Id = format.Id,
                    Name = format.Name,
                    Acronym = format.Acronym
                };


                var latestBanlist = format.Banlist.OrderByDescending(b => b.ReleaseDate).FirstOrDefault();

                if (latestBanlist != null)
                {
                    model.Acronym = $"{format.Acronym} - {latestBanlist.ReleaseDate:MMM d, yyyy}";
                    model.LatestBanlist = new BanlistOutputModel
                    {
                        Id = latestBanlist.Id,
                        FormatId = latestBanlist.FormatId,
                        Name = latestBanlist.Name,
                        ReleaseDate = latestBanlist.ReleaseDate,
                        Cards = latestBanlist.BanlistCard.Select(blc => new BanlistCardOutputModel { BanlistId = blc.BanlistId, CardId = blc.CardId, Limit = blc.Limit.Name.ToLower() })
                    };
                }


                return model;
            })
            .Where(bl => bl.LatestBanlist != null);
        }
    }
}