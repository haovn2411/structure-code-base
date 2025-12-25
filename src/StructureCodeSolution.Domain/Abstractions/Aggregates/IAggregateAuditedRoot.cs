using StructureCodeSolution.Domain.Abstractions.Entities;

namespace StructureCodeSolution.Domain.Abstractions.Aggregates
{
    public interface IAggregateAuditedRoot : IAggregateRoot, IAuditable
    {
    }
}
