using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StructureCodeSolution.Domain.Aggregates.Courses;
using StructureCodeSolution.Persistence.Constants;

namespace StructureCodeSolution.Persistence.Configurations.Courses
{
    internal sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable(TableNames.Courses);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired(true)
                .HasMaxLength(200);

            builder.Property(x => x.SummaryDescription)
                .HasMaxLength(1000);

            // Value Object: Money
            builder.OwnsOne(x => x.Price, priceBuilder =>
            {
                priceBuilder.Property(p => p.Amount)
                    .HasColumnName("Price")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                priceBuilder.Property(p => p.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .HasDefaultValue("USD")
                    .IsRequired();
            });

            // Value Object: Rating
            builder.OwnsOne(x => x.Rating, ratingBuilder =>
            {
                ratingBuilder.Property(r => r.StarRating)
                    .HasColumnName("StarRating")
                    .HasColumnType("decimal(3,2)")
                    .HasDefaultValue(0);

                ratingBuilder.Property(r => r.NumberOfRatings)
                    .HasColumnName("NumberOfRatings")
                    .HasDefaultValue(0);
            });

            // Value Object: CourseStatistics
            builder.OwnsOne(x => x.Statistics, statsBuilder =>
            {
                statsBuilder.Property(s => s.NumberOfStudents)
                    .HasColumnName("NumberOfStudents")
                    .HasDefaultValue(0);

                statsBuilder.Property(s => s.NumberOfComments)
                    .HasColumnName("NumberOfComments")
                    .HasDefaultValue(0);

                statsBuilder.Property(s => s.NumberOfLessons)
                    .HasColumnName("NumberOfLessons")
                    .HasDefaultValue(0);

                statsBuilder.Property(s => s.NumberOfHours)
                    .HasColumnName("NumberOfHours")
                    .HasDefaultValue(0);
            });

            builder.Property(x => x.ImageCode)
                .HasMaxLength(500);

            // Foreign Keys
            builder.Property(x => x.CategoryId);
            builder.Property(x => x.LevelId);



            // Relationships - One-to-Many v?i Video (s? d?ng backing field)
            builder.HasMany(x => x.Videos)
                .WithOne()
                .HasForeignKey("CourseId")  // Shadow property
                .OnDelete(DeleteBehavior.Cascade);

            // Ignore Domain Events
            builder.Ignore(x => x.DomainEvents);

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.CategoryId);
            builder.HasIndex(x => x.LevelId);
        }
    }
}