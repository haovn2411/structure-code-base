using MediatR;
using StructureCodeSolution.Domain.Abstractions.Events;

namespace StructureCodeSolution.Application.Abstractions.Events.AdapterMediatR
{
    public sealed class DomainEventNotification<TDomainEvent> : INotification
        where TDomainEvent : IDomainEvent
    {
        public TDomainEvent DomainEvent { get; }

        public DomainEventNotification(TDomainEvent domainEvent)
        {
            DomainEvent = domainEvent;
        }
    }
}
