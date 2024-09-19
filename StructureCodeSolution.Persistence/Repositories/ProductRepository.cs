using StructureCodeSolution.Domain.Abstractions.Repositories;
using StructureCodeSolution.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureCodeSolution.Persistence.Repositories
{
    public class ProductRepository : RepositoryBase<Product, Guid>, IProductRepository
    {
        public ProductRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
        }
    }
}
