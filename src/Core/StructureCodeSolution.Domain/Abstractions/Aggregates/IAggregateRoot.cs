using StructureCodeSolution.Domain.Abstractions.Events;

namespace StructureCodeSolution.Domain.Abstractions.Aggregates
{
    public interface IAggregateRoot
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
