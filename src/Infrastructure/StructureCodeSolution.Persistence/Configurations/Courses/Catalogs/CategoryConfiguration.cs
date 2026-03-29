using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StructureCodeSolution.Domain.Aggregates.Courses.Catalogs.Categories;
using StructureCodeSolution.Persistence.Constants;

namespace StructureCodeSolution.Persistence.Configurations.Courses.Catalogs
{
    internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(TableNames.Categories);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .IsRequired(true)
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.IconCode)
                .HasMaxLength(100);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // Unique constraint
            builder.HasIndex(x => x.Name)
                .IsUnique();

            // Index for active categories
            builder.HasIndex(x => x.IsActive);
        }
    }
}
