using System.Collections.Generic;
using dueltank.application.Models.Types.Output;
using MediatR;

namespace dueltank.application.Queries.AllTypes
{
    public class AllTypesQuery : IRequest<IEnumerable<TypeOutputModel>>
    {
    }
}