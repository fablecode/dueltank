using System.Collections.Generic;
using dueltank.application.Models.Tips.Output;
using MediatR;

namespace dueltank.application.Queries.TipsByCardId
{
    public class TipsByCardIdQuery : IRequest<IEnumerable<TipSectionOutputModel>>
    {
        public long CardId { get; set; }
    }
}