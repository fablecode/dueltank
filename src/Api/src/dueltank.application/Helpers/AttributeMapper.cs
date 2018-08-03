using dueltank.application.Models.Attributes.Output;
using dueltank.core.Models.Db;
using System.Collections.Generic;
using System.Linq;

namespace dueltank.application.Helpers
{
    public static class AttributeMapper
    {
        public static IEnumerable<AttributeOutputModel> MapToOutputModels(this IList<Attribute> attributes)
        {
            if (attributes == null || !attributes.Any())
                return new AttributeOutputModel[0];

            return attributes.Select(a => new AttributeOutputModel
            {
                Id = a.Id,
                Name = a.Name,
            });
        }
    }
}