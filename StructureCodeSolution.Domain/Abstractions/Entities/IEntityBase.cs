using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureCodeSolution.Domain.Abstractions.Entities
{
    public interface IEntityBase<TKey>
    {
        TKey Id { get; }
    }
}
