using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StructureCodeSolution.Domain.Aggregates.Product;
using StructureCodeSolution.Persistence.Constants;

namespace StructureCodeSolution.Persistence.Configurations
{
    internal class ProductConfigration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(TableNames.Product);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired(true)
                .HasMaxLength(200);
            builder.Property(x => x.Description).IsRequired(true)
                .HasMaxLength(200);
            builder.Property(x => x.Price).IsRequired(true)
                .HasDefaultValue(0);
        }
    }
}
