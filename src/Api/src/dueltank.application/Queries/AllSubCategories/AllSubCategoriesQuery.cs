using System.Collections.Generic;
using dueltank.application.Models.SubCategory.Output;
using MediatR;

namespace dueltank.application.Queries.AllSubCategories
{
    public class AllSubCategoriesQuery : IRequest<IEnumerable<SubCategoryOutputModel>>
    {
    }
}