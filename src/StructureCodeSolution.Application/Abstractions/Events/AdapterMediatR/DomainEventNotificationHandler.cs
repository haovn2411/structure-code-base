using MediatR;
using StructureCodeSolution.Application.Abstractions.Events.Interfaces;
using StructureCodeSolution.Domain.Abstractions.Events;

namespace StructureCodeSolution.Application.Abstractions.Events.AdapterMediatR
{
    public sealed class DomainEventNotificationHandler<TDomainEvent> : INotificationHandler<DomainEventNotification<TDomainEvent>>
        where TDomainEvent : IDomainEvent
    {
        private readonly IEnumerable<IDomainEventHandler<TDomainEvent>> _handlers;

        public DomainEventNotificationHandler(IEnumerable<IDomainEventHandler<TDomainEvent>> handlers)
        {
            _handlers = handlers;
        }

        public async Task Handle(
            DomainEventNotification<TDomainEvent> notification,
            CancellationToken cancellationToken)
        {
            foreach (var handler in _handlers)
            {
                await handler.Handle(notification.DomainEvent, cancellationToken);
            }
        }
    }
}
