using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StructureCodeSolution.Domain.Aggregates.Courses;
using StructureCodeSolution.Persistence.Constants;

namespace StructureCodeSolution.Persistence.Configurations.Courses
{
    internal sealed class VideoConfiguration : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.ToTable(TableNames.Videos);

            builder.HasKey(x => x.Id);

            // Shadow property cho CourseId (không có trong domain model)
            builder.Property<Guid>("CourseId")
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired(true)
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.Duration)
                .IsRequired(true);

            builder.Property(x => x.Order)
                .IsRequired(true);

            builder.Property(x => x.IsPublished)
                .HasDefaultValue(false);

            builder.Property(x => x.VideoUrl)
                .HasMaxLength(500);

            // Unique constraint on CourseId + Order
            builder.HasIndex("CourseId", nameof(Video.Order))
                .IsUnique();

            // Index for queries
            builder.HasIndex("CourseId");
        }
    }
}
