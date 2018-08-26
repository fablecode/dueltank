using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace dueltank.application.Queries.CardImageByName
{
    public class CardImageByNameQueryHandler : IRequestHandler<CardImageByNameQuery, CardImageByNameResult>
    {
        public Task<CardImageByNameResult> Handle(CardImageByNameQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}