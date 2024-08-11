using StructureCodeSolution.Domain.Abstractions.Aggregates;

namespace StructureCodeSolution.Domain.Abstractions
{
    public abstract class AggregateAuditedRoot<TKey> : AggregateRoot<TKey>, IAggregateAuditedRoot
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
