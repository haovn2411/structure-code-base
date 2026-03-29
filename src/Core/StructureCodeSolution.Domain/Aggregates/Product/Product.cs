using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Product
{
    public class Product : AggregateAuditedRoot<Guid>
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }

        private Product()
        {

        }

        private Product(Guid id, decimal price, string description)
        {
            Id = id;
            Price = price;
            Description = description;
        }

        public static Product CreateProduct(decimal price, string description)
        {
            var product = new Product(Guid.NewGuid(), price, description);
            return product;
        }

    }
}
