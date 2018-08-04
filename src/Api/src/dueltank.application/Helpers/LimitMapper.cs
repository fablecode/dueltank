using dueltank.application.Models.Category.Output;
using dueltank.core.Models.Db;
using System.Collections.Generic;
using System.Linq;
using dueltank.application.Models.Limits.Output;

namespace dueltank.application.Helpers
{
    public static class LimitMapper
    {
        public static IEnumerable<LimitOutputModel> MapToOutputModels(this IList<Limit> limits)
        {
            if (limits == null || !limits.Any())
                return new LimitOutputModel[0];

            return limits.Select(c => new LimitOutputModel
            {
                Id = c.Id,
                Name = c.Name,
            });
        }
    }
}