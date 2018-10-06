using System.Collections.Generic;
using dueltank.application.Models.Rulings.Output;
using MediatR;

namespace dueltank.application.Queries.RulingsByCardId
{
    public class RulingsByCardIdQuery : IRequest<IEnumerable<RulingSectionOutputModel>>
    {
        public long CardId { get; set; }
    }
}