using System.Collections.Generic;
using System.Linq;
using dueltank.application.Models.SubCategory.Output;
using dueltank.core.Models.Db;

namespace dueltank.application.Helpers
{
    public static class SubCategoryMapper
    {
        public static IEnumerable<SubCategoryOutputModel> MapToOutputModels(this IList<SubCategory> formats)
        {
            if (formats == null || !formats.Any())
                return new SubCategoryOutputModel[0];

            return formats.Select(c => new SubCategoryOutputModel
            {
                Id = c.Id,
                CategoryId = c.CategoryId,
                Name = c.Name,
            });
        }
    }
}