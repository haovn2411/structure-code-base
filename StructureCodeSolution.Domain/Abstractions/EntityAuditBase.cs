using StructureCodeSolution.Domain.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureCodeSolution.Domain.Abstractions
{
    public abstract class EntityAuditBase<TKey> : EntityBase<TKey>, IEntityAuditBase<TKey>
    {
        public DateTimeOffset CreatedDate { get ; set ; }
        public DateTimeOffset? ModifiedDate { get ; set ; }
        public Guid CreatedBy { get ; set ; }
        public Guid? ModifiedBy { get ; set ; }
        public bool IsDeleted { get ; set ; }
        public DateTimeOffset? DeletedAt { get ; set ; }
    }
}
