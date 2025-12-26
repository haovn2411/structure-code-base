namespace StructureCodeSolution.Domain.Abstractions.Events
{
    public interface IDomainEvent
    {
        Guid EventId { get; }
        DateTime OccurredOn { get; }
    }
}
