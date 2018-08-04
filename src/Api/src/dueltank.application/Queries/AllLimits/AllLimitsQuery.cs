using System.Collections.Generic;
using dueltank.application.Models.Limits.Output;
using MediatR;

namespace dueltank.application.Queries.AllLimits
{
    public class AllLimitsQuery : IRequest<IEnumerable<LimitOutputModel>>
    {
    }
}