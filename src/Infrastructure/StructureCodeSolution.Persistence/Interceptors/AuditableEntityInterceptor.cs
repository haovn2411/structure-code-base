using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using StructureCodeSolution.Application.Abstractions.Identity;
using StructureCodeSolution.Domain.Abstractions.Entities;

namespace StructureCodeSolution.Persistence.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        private readonly IServiceProvider _serviceProvider;

        public AuditableEntityInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? context)
        {
            if (context == null) { return; }

            var serviceScope = _serviceProvider.CreateScope();
            var currentUserService = serviceScope.ServiceProvider.GetRequiredService<ICurrentUserService>();
            var userId = currentUserService.UserId;
            var now = DateTimeOffset.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries())
            {
                Console.WriteLine(entry.Entity + ":");
                Console.WriteLine(entry.State);
                // Date Tracking
                if (entry.Entity is IDateTracking dateTracking)
                {
                    if (entry.State == EntityState.Added)
                    {
                        dateTracking.CreatedDate = now;
                    }
                    else if (entry.State == EntityState.Modified
                        && !entry.Property(nameof(ISoftDelete.IsDeleted)).IsModified)
                    {
                        dateTracking.ModifiedDate = now;
                    }
                }

                // User Tracking
                if (entry.Entity is IUserTracking userTracking)
                {
                    if (entry.State == EntityState.Added)
                    {
                        userTracking.CreatedBy = userId;
                    }
                    else if (entry.State == EntityState.Modified
                        && !entry.Property(nameof(ISoftDelete.IsDeleted)).IsModified)
                    {
                        userTracking.ModifiedBy = userId;
                    }
                }

                // Soft Delete
                if (entry.Entity is ISoftDelete softDelete
                    && entry.State == EntityState.Modified
                    && entry.Property(nameof(ISoftDelete.IsDeleted)).IsModified)
                {
                    var isDeleted = entry.Property(nameof(ISoftDelete.IsDeleted)).CurrentValue as bool?;
                    if (isDeleted.HasValue && isDeleted.Value)
                    {
                        softDelete.DeletedAt = now;
                    }
                }
            }
        }
    }
}