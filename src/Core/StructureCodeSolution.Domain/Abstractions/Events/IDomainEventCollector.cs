namespace StructureCodeSolution.Domain.Abstractions.Events
{
    public interface IDomainEventCollector
    {
        IReadOnlyCollection<IDomainEvent> GetCapturedEvents();
        void ClearEvents();
    }
}
