using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dueltank.application.Helpers;
using dueltank.application.Models.Archetypes.Output;
using dueltank.core.Services;
using MediatR;

namespace dueltank.application.Queries.ArchetypeById
{
    public class ArchetypeByIdQueryHandler : IRequestHandler<ArchetypeByIdQuery, ArchetypeSearchOutputModel>
    {
        private readonly IArchetypeService _archetypeService;

        public ArchetypeByIdQueryHandler(IArchetypeService archetypeService)
        {
            _archetypeService = archetypeService;
        }

        public async Task<ArchetypeSearchOutputModel> Handle(ArchetypeByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _archetypeService.ArchetypeById(request.ArchetypeId);

            if (result == null)
                return null;

            var response = ArchetypeSearchOutputModel.From(result.Archetype);
            response.Cards = result.Cards.Select(CardSearchMapperHelper.MapToCardOutputModel).ToList();
            response.TotalCards = result.Cards.Count;

            return response;
        }
    }
}