using StructureCodeSolution.Domain.Abstractions.Events;

namespace StructureCodeSolution.Domain.Abstractions
{
    public abstract class DomainEvent : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
