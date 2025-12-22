using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Domain.Aggregates.Product
{
    public class Product : EntityAuditBase<Guid>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

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
