using StructureCodeSolution.Domain.Abstractions.Aggregates;
using StructureCodeSolution.Domain.Abstractions.Events;

namespace StructureCodeSolution.Persistence.Events
{
    public class DomainEventCollector : IDomainEventCollector
    {
        private readonly ApplicationDBContext _context;

        public DomainEventCollector(ApplicationDBContext context)
        {
            _context = context;
        }

        public IReadOnlyCollection<IDomainEvent> GetCapturedEvents()
        {
            // Lục soát ChangeTracker để tìm các AggregateRoot có chứa Event
            return _context.ChangeTracker.Entries<IAggregateRoot>()
                .Select(x => x.Entity)
                .SelectMany(aggregate =>
                {
                    var events = aggregate.DomainEvents.ToList();
                    return events;
                })
                .ToList();
        }

        public void ClearEvents()
        {
            var aggregates = _context.ChangeTracker.Entries<IAggregateRoot>()
                .Select(x => x.Entity);

            foreach (var aggregate in aggregates)
            {
                aggregate.ClearDomainEvents();
            }
        }
    }
}
