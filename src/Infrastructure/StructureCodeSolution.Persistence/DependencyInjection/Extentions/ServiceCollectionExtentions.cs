using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StructureCodeSolution.Application.Abstractions.Events.Interfaces;
using StructureCodeSolution.Application.Abstractions.Identity;
using StructureCodeSolution.Domain.Abstractions;
using StructureCodeSolution.Domain.Abstractions.Repositories;
using StructureCodeSolution.Domain.Abstractions.Repositories.RepositoryBase;
using StructureCodeSolution.Domain.Aggregates.Identity;
using StructureCodeSolution.Persistence.DependencyInjection.Options;
using StructureCodeSolution.Persistence.Events;
using StructureCodeSolution.Persistence.Interceptors;
using StructureCodeSolution.Persistence.Repositories;
using StructureCodeSolution.Persistence.Services;

namespace StructureCodeSolution.Persistence.DependencyInjection.Extentions
{
    public static class ServiceCollectionExtentions
    {
        public static void AddSQLServerPersistence(this IServiceCollection services)
        {
            services.AddDbContextPool<DbContext, ApplicationDBContext>((provider, builder) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var options = provider.GetRequiredService<IOptionsMonitor<SqlServerRetryOptions>>();
                var auditableEntityInterceptor = provider.GetRequiredService<AuditableEntityInterceptor>();

                builder
                    .EnableDetailedErrors(true)
                    .EnableSensitiveDataLogging(true)
                    .UseSqlServer(
                        connectionString: configuration.GetConnectionString("MyDbContext"),
                        sqlServerOptionsAction: optionsBuilder
                        => optionsBuilder.ExecutionStrategy(
                                dependencies => new SqlServerRetryingExecutionStrategy(
                                    dependencies: dependencies,
                                    maxRetryCount: options.CurrentValue.MaxRetryCount,
                                    maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                                    errorNumbersToAdd: options.CurrentValue.ErrorNumbersToAdd))
                            .MigrationsAssembly(typeof(ApplicationDBContext).Assembly.GetName().Name))
                    .AddInterceptors(auditableEntityInterceptor);
            });

            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Lockout.AllowedForNewUsers = true; // Default true
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Default 5
                opt.Lockout.MaxFailedAccessAttempts = 3; // Default 5
            })
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.AllowedForNewUsers = true; // Default true
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Default 5
                options.Lockout.MaxFailedAccessAttempts = 3; // Default 5
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.Lockout.AllowedForNewUsers = true;
            });
        }

        public static void AddRepositoryPersistence(this IServiceCollection services)
        {
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
            services.AddTransient<IDeviceRepository, DeviceRepository>();
            services.AddTransient<ICourseRepository, CourseRepository>();
        }

        public static void AddInterceptorPersistence(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<AuditableEntityInterceptor>();
        }

        public static OptionsBuilder<SqlServerRetryOptions> ConfigureSqlServerRetryOptions(this IServiceCollection services, IConfigurationSection section)
            => services
                .AddOptions<SqlServerRetryOptions>()
                .Bind(section)
                .ValidateDataAnnotations()
                .ValidateOnStart();

        public static IServiceCollection AddDomainEventCollector(this IServiceCollection services)
            => services.AddScoped<IDomainEventCollector, DomainEventCollector>();

        public static IServiceCollection AddUserService(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}