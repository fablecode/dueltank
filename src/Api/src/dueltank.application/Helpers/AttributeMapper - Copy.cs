using dueltank.application.Models.Types.Output;
using dueltank.core.Models.Db;
using System.Collections.Generic;
using System.Linq;

namespace dueltank.application.Helpers
{
    public static class TypeMapper
    {
        public static IEnumerable<TypeOutputModel> MapToOutputModels(this IList<Type> types)
        {
            if (types == null || !types.Any())
                return new TypeOutputModel[0];

            return types.Select(a => new TypeOutputModel
            {
                Id = a.Id,
                Name = a.Name,
            });
        }
    }
}