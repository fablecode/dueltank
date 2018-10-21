using dueltank.application.Enums;
using dueltank.application.Models.Banlists.Output;
using MediatR;

namespace dueltank.application.Queries.LatestBanlist
{
    public class LatestBanlistQuery : IRequest<LatestBanlistOutputModel>
    {
        public BanlistFormat Format { get; set; }
    }
}