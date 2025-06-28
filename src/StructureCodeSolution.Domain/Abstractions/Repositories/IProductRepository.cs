using StructureCodeSolution.Domain.Abstractions.Repositories.RepositoryBase;
using StructureCodeSolution.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureCodeSolution.Domain.Abstractions.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product, Guid>
    {
    }
}
