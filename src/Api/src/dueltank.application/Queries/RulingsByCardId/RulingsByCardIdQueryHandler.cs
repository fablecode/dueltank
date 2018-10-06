using dueltank.application.Models.Rulings.Output;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dueltank.core.Services;

namespace dueltank.application.Queries.RulingsByCardId
{
    public class RulingsByCardIdQueryHandler : IRequestHandler<RulingsByCardIdQuery, IEnumerable<RulingSectionOutputModel>>
    {
        private readonly IRulingService _rulingService;

        public RulingsByCardIdQueryHandler(IRulingService rulingService)
        {
            _rulingService = rulingService;
        }

        public async Task<IEnumerable<RulingSectionOutputModel>> Handle(RulingsByCardIdQuery request, CancellationToken cancellationToken)
        {
            var response = new List<RulingSectionOutputModel>();
            var rulingSections = await _rulingService.GetRulingsByCardId(request.CardId);

            if (rulingSections.Any())
            {
                foreach (var rulingSection in rulingSections)
                {
                    var rulingSectionOutputModel = new RulingSectionOutputModel
                    {
                        CardId = rulingSection.CardId,
                        Name = rulingSection.Name
                    };

                    foreach (var tip in rulingSection.Ruling)
                    {
                        rulingSectionOutputModel.Rulings.Add(new RulingOutputModel { Text = tip.Text });
                    }

                    response.Add(rulingSectionOutputModel);
                }
            }
            else
            {
                response.Add(new RulingSectionOutputModel { Name = "No rulings for this card yet....." });
            }

            return response;
        }
    }
}