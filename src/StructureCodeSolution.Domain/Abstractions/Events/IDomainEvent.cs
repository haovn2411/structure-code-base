using MediatR;

namespace StructureCodeSolution.Domain.Abstractions.Events
{
    public interface IDomainEvent : INotification
    {
        Guid EventId { get; }
        DateTime OccurredOn { get; }
        string EventType { get; }
        int Version { get; }
    }
}
