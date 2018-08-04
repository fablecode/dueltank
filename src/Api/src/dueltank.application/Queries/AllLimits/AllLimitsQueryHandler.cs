using dueltank.application.Helpers;
using dueltank.application.Models.Limits.Output;
using dueltank.core.Services;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace dueltank.application.Queries.AllLimits
{
    public class AllLimitsQueryHandler : IRequestHandler<AllLimitsQuery, IEnumerable<LimitOutputModel>>
    {
        private readonly ILimitService _limitService;

        public AllLimitsQueryHandler(ILimitService limitService)
        {
            _limitService = limitService;
        }

        public async Task<IEnumerable<LimitOutputModel>> Handle(AllLimitsQuery request, CancellationToken cancellationToken)
        {
            var result = await _limitService.AllLimits();

            return result.MapToOutputModels();
        }
    }
}