using MediatR;

namespace dueltank.application.Queries.CardImageByName
{
    public class CardImageByNameQuery : IRequest<CardImageByNameResult>
    {
        public string Name { get; set; }
    }
}