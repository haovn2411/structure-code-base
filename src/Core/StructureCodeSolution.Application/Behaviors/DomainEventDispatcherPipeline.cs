using MediatR;
using Microsoft.Extensions.Logging;
using StructureCodeSolution.Application.Abstractions.Events.AdapterMediatR;
using StructureCodeSolution.Application.Abstractions.Events.Interfaces;
using StructureCodeSolution.Domain.Abstractions.Events;

namespace StructureCodeSolution.Application.Behaviors
{
    public class DomainEventDispatcherPipeline<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IDomainEventCollector _domainEventCollector;
        private readonly IPublisher _publisher;
        private readonly ILogger<DomainEventDispatcherPipeline<TRequest, TResponse>> _logger;

        public DomainEventDispatcherPipeline(
            IDomainEventCollector domainEventCollector,
            IPublisher publisher,
            ILogger<DomainEventDispatcherPipeline<TRequest, TResponse>> logger)
        {
            _domainEventCollector = domainEventCollector;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var response = await next();

            var domainEvents = _domainEventCollector.GetCapturedEvents();

            if (!domainEvents.Any())
            {
                return response;
            }

            _domainEventCollector.ClearEvents();

            await DispatchDomainEventsAsync(domainEvents, cancellationToken);

            return response;
        }
        private async Task DispatchDomainEventsAsync(
            IReadOnlyCollection<IDomainEvent> domainEvents,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Dispatching {Count} domain events for {RequestName}",
                domainEvents.Count,
                typeof(TRequest).Name);

            foreach (var domainEvent in domainEvents)
            {
                try
                {
                    _logger.LogDebug(
                        "Dispatching domain event: {EventType}",
                        domainEvent.GetType().Name);

                    await PublishDomainEventAsync(domainEvent, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Failed to dispatch domain event: {EventType}",
                        domainEvent.GetType().Name);
                }
            }
        }

        private async Task PublishDomainEventAsync(
            IDomainEvent domainEvent,
            CancellationToken cancellationToken)
        {
            var notificationType = typeof(DomainEventNotification<>)
                .MakeGenericType(domainEvent.GetType());

            var notification = Activator.CreateInstance(notificationType, domainEvent);

            if (notification is INotification mediatRNotification)
            {
                await _publisher.Publish(mediatRNotification, cancellationToken);
            }
        }
    }
}
