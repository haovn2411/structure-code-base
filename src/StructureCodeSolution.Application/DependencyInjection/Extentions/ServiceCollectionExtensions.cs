using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StructureCodeSolution.Application.Abstractions.Events.AdapterMediatR;
using StructureCodeSolution.Application.Behaviors;

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
            .AddValidatorsFromAssembly(Contract.AssemblyReference.Assembly, includeInternalTypes: true);

        public static IServiceCollection AddConfigureAutoMapper(this IServiceCollection services)
            => services.AddAutoMapper(AssemblyReference.Assembly);


        public static IServiceCollection AddDomainEventNotification(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DomainEventNotificationHandler<>).Assembly);
                cfg.RegisterGenericHandlers = true;
            });
            return services;
        }
    }
}
