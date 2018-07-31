using System.Collections.Generic;
using dueltank.application.Models.Formats.Output;
using MediatR;

namespace dueltank.application.Queries.AllFormats
{
    public class AllFormatsQuery : IRequest<IEnumerable<FormatOutputModel>>
    {
    }
}