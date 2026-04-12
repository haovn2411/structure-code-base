using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StructureCodeSolution.Domain.Aggregates.Courses;
using StructureCodeSolution.Domain.Aggregates.Courses.Catalogs.Categories;
using StructureCodeSolution.Domain.Aggregates.Courses.Catalogs.Levels;
using StructureCodeSolution.Domain.Aggregates.Devices;
using StructureCodeSolution.Domain.Aggregates.Identity;
using StructureCodeSolution.Domain.Aggregates.Product;

namespace StructureCodeSolution.Persistence
{
    public sealed class ApplicationDBContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
                    => builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Device> Devices { get; set; }

        // Course Aggregate
        public DbSet<Course> Courses { get; set; }

        public DbSet<Video> Videos { get; set; }

        // Reference Data
        public DbSet<Category> Categories { get; set; }

        public DbSet<Level> Levels { get; set; }
    }
}