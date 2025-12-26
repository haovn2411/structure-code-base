using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StructureCodeSolution.Domain.Aggregates.Devices;
using StructureCodeSolution.Persistence.Constants;

namespace StructureCodeSolution.Persistence.Configurations
{
    internal class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.ToTable(TableNames.Device);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired(true)
                .HasMaxLength(200);
            builder.Property(x => x.Description).IsRequired(true)
                .HasMaxLength(200);
        }
    }
}
