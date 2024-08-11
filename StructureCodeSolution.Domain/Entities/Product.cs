using StructureCodeSolution.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureCodeSolution.Domain.Entities
{
    public class Product : EntityAuditSoftDeleteBase<Guid>
    {
        public string Name {  get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
