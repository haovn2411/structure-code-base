using StructureCodeSolution.Domain.Abstractions.Events;

namespace StructureCodeSolution.Application.Abstractions.Events.Interfaces
{
    public interface IDomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);
    }
}
