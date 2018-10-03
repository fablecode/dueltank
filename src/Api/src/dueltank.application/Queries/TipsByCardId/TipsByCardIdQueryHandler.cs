using dueltank.application.Models.Tips.Output;
using dueltank.core.Services;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dueltank.application.Queries.TipsByCardId
{
    public class TipsByCardIdQueryHandler : IRequestHandler<TipsByCardIdQuery, IEnumerable<TipSectionOutputModel>>
    {
        private readonly ITipService _tipService;

        public TipsByCardIdQueryHandler(ITipService tipService)
        {
            _tipService = tipService;
        }

        public async Task<IEnumerable<TipSectionOutputModel>> Handle(TipsByCardIdQuery request, CancellationToken cancellationToken)
        {
            var response = new List<TipSectionOutputModel>();
            var tips = await _tipService.GetTipsByCardId(request.CardId);

            if (tips.Any())
            {
                foreach (var tipSection in tips)
                {
                    var tipSectionOutputModel = new TipSectionOutputModel
                    {
                        CardId = tipSection.CardId,
                        Name = tipSection.Name
                    };

                    foreach (var tip in tipSection.Tip)
                    {
                        tipSectionOutputModel.Tips.Add(new TipOutputModel{ Text = tip.Text });
                    }

                    response.Add(tipSectionOutputModel);
                }
            }
            else
            {
                response.Add(new TipSectionOutputModel{ Name = "No tips for this card yet....."});
            }

            return response;
        }
    }
}