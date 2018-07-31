using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Helpers;
using dueltank.application.Models.Formats.Output;
using dueltank.core.Services;
using MediatR;

namespace dueltank.application.Queries.AllFormats
{
    public class AllFormatsQueryHandler : IRequestHandler<AllFormatsQuery, IEnumerable<FormatOutputModel>>
    {
        private readonly IFormatService _formatService;

        public AllFormatsQueryHandler(IFormatService formatService)
        {
            _formatService = formatService;
        }

        public async Task<IEnumerable<FormatOutputModel>> Handle(AllFormatsQuery request, CancellationToken cancellationToken)
        {
            var formats = await _formatService.AllFormats();

            return formats.MapToOutputModels();
        }
    }
}