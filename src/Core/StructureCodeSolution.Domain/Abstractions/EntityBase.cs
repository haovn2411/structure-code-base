using StructureCodeSolution.Domain.Abstractions.Entities;

namespace StructureCodeSolution.Domain.Abstractions
{
    public abstract class EntityBase<TKey> : IEntityBase<TKey>
    {
        public TKey Id { get; set; }
    }
}
