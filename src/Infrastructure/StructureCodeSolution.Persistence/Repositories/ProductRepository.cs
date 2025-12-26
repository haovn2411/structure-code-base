using StructureCodeSolution.Domain.Abstractions.Repositories;
using StructureCodeSolution.Domain.Aggregates.Product;

namespace StructureCodeSolution.Persistence.Repositories
{
    public class ProductRepository : RepositoryBase<Product, Guid>, IProductRepository
    {
        public ProductRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
        }
    }
}
