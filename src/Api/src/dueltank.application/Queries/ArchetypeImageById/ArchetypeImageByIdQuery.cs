using MediatR;

namespace dueltank.application.Queries.ArchetypeImageById
{
    public sealed class ArchetypeImageByIdQuery : IRequest<ArchetypeImageByIdQueryResult>
    {
        public long ArchetypeId { get; set; }
    }
}