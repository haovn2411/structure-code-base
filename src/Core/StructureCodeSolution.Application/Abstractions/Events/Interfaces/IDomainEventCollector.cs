using StructureCodeSolution.Domain.Abstractions.Events;

namespace StructureCodeSolution.Application.Abstractions.Events.Interfaces
{
    public interface IDomainEventCollector
    {
        IReadOnlyCollection<IDomainEvent> GetCapturedEvents();
        void ClearEvents();
    }
}
