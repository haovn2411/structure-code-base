using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StructureCodeSolution.Domain.Aggregates.Courses.Catalogs.Levels;
using StructureCodeSolution.Persistence.Constants;

namespace StructureCodeSolution.Persistence.Configurations.Courses.Catalogs
{
    internal sealed class LevelConfiguration : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> builder)
        {
            builder.ToTable(TableNames.Levels);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .IsRequired(true)
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Order)
                .IsRequired(true)
                .HasDefaultValue(0);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // Unique constraints
            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasIndex(x => x.Order)
                .IsUnique();

            // Index for active levels
            builder.HasIndex(x => x.IsActive);
        }
    }
}
