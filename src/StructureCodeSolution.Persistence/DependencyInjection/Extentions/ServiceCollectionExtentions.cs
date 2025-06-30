using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StructureCodeSolution.Domain.Abstractions;
using StructureCodeSolution.Domain.Abstractions.Repositories.RepositoryBase;
using StructureCodeSolution.Domain.Aggregates.Identity;
using StructureCodeSolution.Persistence.DependencyInjection.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureCodeSolution.Persistence.DependencyInjection.Extentions
{
    public static class ServiceCollectionExtentions
    {
        public static void AddSQLServerPersistence(this IServiceCollection services)
        {
            services.AddDbContextPool<DbContext, ApplicationDBContext>((provider, builder) =>
            {
                // dùng để lấy configuration từ appsetting và map vào object
                //var passwordValidatorOptions =
                //    services.BuildServiceProvider().GetRequiredService<IOptionsMonitor<PasswordValidatorOptions>>();
                //var auditableInterceptor = provider.GetService<UpdateAuditableEntitiesInterceptor>();
                var configuration = provider.GetRequiredService<IConfiguration>();
                var options = provider.GetRequiredService<IOptionsMonitor<SqlServerRetryOptions>>();

                builder
                    .EnableDetailedErrors(true)
                    .EnableSensitiveDataLogging(true)
                    .UseLazyLoadingProxies(true) // => If UseLazyLoadingProxies, all of the navigation fields should be VIRTUAL
                    .UseSqlServer(
                        connectionString: configuration.GetConnectionString("MyDbContext"),
                        sqlServerOptionsAction: optionsBuilder
                        => optionsBuilder.ExecutionStrategy(
                                dependencies => new SqlServerRetryingExecutionStrategy(
                                    dependencies: dependencies,
                                    maxRetryCount: options.CurrentValue.MaxRetryCount,
                                    maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                                    errorNumbersToAdd: options.CurrentValue.ErrorNumbersToAdd))
                            .MigrationsAssembly(typeof(ApplicationDBContext).Assembly.GetName().Name));
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
        }

        public static void AddInterceptorPersistence(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static OptionsBuilder<SqlServerRetryOptions> ConfigureSqlServerRetryOptions(this IServiceCollection services, IConfigurationSection section)
            => services
                .AddOptions<SqlServerRetryOptions>()
                .Bind(section)
                .ValidateDataAnnotations()
                .ValidateOnStart();

    }
}
