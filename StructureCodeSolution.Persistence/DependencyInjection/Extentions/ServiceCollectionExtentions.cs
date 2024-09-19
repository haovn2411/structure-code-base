using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StructureCodeSolution.Domain.Abstractions;
using StructureCodeSolution.Domain.Abstractions.Repositories.RepositoryBase;
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
                builder
                    .UseSqlServer(
                    connectionString: configuration.GetConnectionString("MyDbContext"),
                    sqlServerOptionsAction: optionsBuilder 
                        => optionsBuilder.MigrationsAssembly(typeof(ApplicationDBContext).Assembly.GetName().Name));
                    //.AddInterceptors(audit)
            });
        }

        public static void AddRepositoryPersistence(this IServiceCollection services)
        {
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));



            //services.AddTransient<IProductRepository, ProductRepository>();

        }

        public static void AddInterceptorPersistence(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        }

    }
}
