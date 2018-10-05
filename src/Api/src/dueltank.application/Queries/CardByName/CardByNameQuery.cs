using dueltank.application.Models.Cards.Output;
using MediatR;

namespace dueltank.application.Queries.CardByName
{
    public class CardByNameQuery : IRequest<CardOutputModel>
    {
        public string Name { get; set; }
    }
}