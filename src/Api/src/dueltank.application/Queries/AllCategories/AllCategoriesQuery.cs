using System.Collections.Generic;
using dueltank.application.Models.Category.Output;
using MediatR;

namespace dueltank.application.Queries.AllCategories
{
    public class AllCategoriesQuery : IRequest<IEnumerable<CategoryOutputModel>>
    {
    }
}