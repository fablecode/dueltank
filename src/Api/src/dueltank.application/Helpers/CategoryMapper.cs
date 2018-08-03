using dueltank.application.Models.Category.Output;
using dueltank.core.Models.Db;
using System.Collections.Generic;
using System.Linq;

namespace dueltank.application.Helpers
{
    public static class CategoryMapper
    {
        public static IEnumerable<CategoryOutputModel> MapToOutputModels(this IList<Category> formats)
        {
            if (formats == null || !formats.Any())
                return new CategoryOutputModel[0];

            return formats.Select(c => new CategoryOutputModel
            {
                Id = c.Id,
                Name = c.Name,
            });
        }
    }
}