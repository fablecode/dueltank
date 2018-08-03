using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Helpers;
using dueltank.application.Models.SubCategory.Output;
using dueltank.core.Models.Db;
using MediatR;

namespace dueltank.application.Queries.AllSubCategories
{
    public class AllSubCategoriesHandler : IRequestHandler<AllSubCategoriesQuery, IEnumerable<SubCategoryOutputModel>>
    {
        private readonly ISubCategoryService _subCategoryService;

        public AllSubCategoriesHandler(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        public async Task<IEnumerable<SubCategoryOutputModel>> Handle(AllSubCategoriesQuery request, CancellationToken cancellationToken)
        {
            var formats = await _subCategoryService.AllSubCategories();

            return formats.MapToOutputModels();
        }
    }

    public interface ISubCategoryService
    {
        Task<IList<SubCategory>> AllSubCategories();
    }
}