using System.Collections.Generic;
using dueltank.application.Models.Attributes.Output;
using MediatR;

namespace dueltank.application.Queries.AllAttributes
{
    public class AllAttributesQuery : IRequest<IEnumerable<AttributeOutputModel>>
    {
    }
}