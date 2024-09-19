using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StructureCodeSolution.Domain.Entities.Product;
using StructureCodeSolution.Persistence.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
