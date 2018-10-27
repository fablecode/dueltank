using dueltank.application.Models.Archetypes.Output;
using MediatR;

namespace dueltank.application.Queries.ArchetypeById
{
    public class ArchetypeByIdQuery : IRequest<ArchetypeSearchOutputModel>
    {
        public long ArchetypeId { get; set; }
    }
}