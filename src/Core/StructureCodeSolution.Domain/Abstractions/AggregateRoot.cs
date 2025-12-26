using StructureCodeSolution.Domain.Abstractions.Aggregates;
using StructureCodeSolution.Domain.Abstractions.Events;

namespace StructureCodeSolution.Domain.Abstractions
{
    public abstract class AggregateRoot<TKey> : EntityBase<TKey>, IAggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

        protected void Raise(IDomainEvent domainEvent)
            => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents()
            => _domainEvents.Clear();
    }
}
