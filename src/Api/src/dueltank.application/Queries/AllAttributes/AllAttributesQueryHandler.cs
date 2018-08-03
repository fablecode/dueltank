using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Helpers;
using dueltank.application.Models.Attributes.Output;
using dueltank.core.Services;
using MediatR;

namespace dueltank.application.Queries.AllAttributes
{
    public class AllAttributesQueryHandler : IRequestHandler<AllAttributesQuery, IEnumerable<AttributeOutputModel>>
    {
        private readonly IAttributeService _attributeService;

        public AllAttributesQueryHandler(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }
        public async Task<IEnumerable<AttributeOutputModel>> Handle(AllAttributesQuery request, CancellationToken cancellationToken)
        {
            var attributes = await _attributeService.AllAttributes();

            return attributes.MapToOutputModels();
        }
    }
}