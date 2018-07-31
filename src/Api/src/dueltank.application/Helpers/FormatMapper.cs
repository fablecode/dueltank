using System.Collections.Generic;
using System.Linq;
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

            return formats.Select(f => new FormatOutputModel
            {
                Id = f.Id,
                Name = f.Name,
                Acronym = f.Acronym
            });
        }
    }
}