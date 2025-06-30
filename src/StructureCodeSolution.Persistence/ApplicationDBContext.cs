using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

    }
}
