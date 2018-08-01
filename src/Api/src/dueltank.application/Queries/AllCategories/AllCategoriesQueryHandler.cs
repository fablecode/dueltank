using dueltank.application.Helpers;
using dueltank.application.Models.Category.Output;
using dueltank.core.Services;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dueltank.application.Queries.AllCategories
{
    public class AllCategoriesQueryHandler : IRequestHandler<AllCategoriesQuery, IEnumerable<CategoryOutputModel>>
    {
        private readonly ICategoryService _categoryService;

        public AllCategoriesQueryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IEnumerable<CategoryOutputModel>> Handle(AllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var formats = await _categoryService.AllCategories();

            return formats.MapToOutputModels();
        }
    }
}