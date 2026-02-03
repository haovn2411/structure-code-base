using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StructureCodeSolution.Application.Abstractions.Events.AdapterMediatR;
using StructureCodeSolution.Application.Abstractions.Events.Interfaces;
using StructureCodeSolution.Application.Behaviors;
using StructureCodeSolution.Domain.Abstractions.Events;

namespace StructureCodeSolution.Application.DependencyInjection.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigureMediatR(this IServiceCollection services)
            => services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingPipelineBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformancePipelineBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(DomainEventDispatcherPipeline<,>))
            .AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);

        public static IServiceCollection AddConfigureAutoMapper(this IServiceCollection services)
            => services.AddAutoMapper(AssemblyReference.Assembly);


        public static IServiceCollection AddDomainEventNotificationHandlers(this IServiceCollection services)
        {
            var assembly = AssemblyReference.Assembly;

            // Find All of types implement IDomainEvent in Domain layer
            var domainEventTypes = Domain.AssemblyReference.Assembly.GetTypes()
                .Where(t => typeof(IDomainEvent).IsAssignableFrom(t)
                            && t.IsClass
                            && !t.IsAbstract
                            && t != typeof(IDomainEvent))
                .ToList();

            // Register DomainEventNotificationHandler<T> for each IDomainEvent type
            foreach (var eventType in domainEventTypes)
            {
                var notificationType = typeof(DomainEventNotification<>).MakeGenericType(eventType);
                var handlerType = typeof(DomainEventNotificationHandler<>).MakeGenericType(eventType);
                var interfaceType = typeof(INotificationHandler<>).MakeGenericType(notificationType);

                services.AddTransient(interfaceType, handlerType);
            }


            // Find All of types implement IDomainEventHandler<T> in Application layer
            var domainEventHandlers = Application.AssemblyReference.Assembly.GetTypes()
                .Where(t => t.IsClass
                            && !t.IsAbstract
                            && !t.IsInterface
                            && !t.IsGenericTypeDefinition)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                           i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>))
                    .Select(i => new
                    {
                        Interface = i,
                        Implementation = t
                    }))
                .ToList();

            foreach (var handler in domainEventHandlers)
            {
                services.AddTransient(handler.Interface, handler.Implementation);
            }

            return services;
        }
    }
}
