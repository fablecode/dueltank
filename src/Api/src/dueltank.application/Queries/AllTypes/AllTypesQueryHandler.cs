using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Helpers;
using dueltank.application.Models.Types.Output;
using dueltank.core.Services;
using MediatR;

namespace dueltank.application.Queries.AllTypes
{
    public class AllTypesQueryHandler : IRequestHandler<AllTypesQuery, IEnumerable<TypeOutputModel>>
    {
        private readonly ITypeService _typeService;

        public AllTypesQueryHandler(ITypeService typeService)
        {
            _typeService = typeService;
        }

        public async Task<IEnumerable<TypeOutputModel>> Handle(AllTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _typeService.AllTypes();

            return types.MapToOutputModels();
        }
    }
}