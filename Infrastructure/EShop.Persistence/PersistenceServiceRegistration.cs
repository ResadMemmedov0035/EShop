using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EShop.Persistence.Contexts;
using Microsoft.Extensions.Configuration;
using EShop.Application.Repositories;
using EShop.Persistence.Repositories;
using EShop.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace EShop.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContext, EShopDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"));
            });

            services.AddScoped(typeof(IWriteRepository<,>), typeof(EFCoreWriteRepository<,>));
            services.AddScoped(typeof(IReadRepository<,>), typeof(EFCoreReadRepository<,>));
            services.AddScoped(typeof(IMapperReadRepository<,>), typeof(EFCoreAutoMapperReadRepository<,>));

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                // Just for ease the testing process. Password must be more secure.
                options.Password.RequiredLength = 5;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<EShopDbContext>()
            .AddDefaultTokenProviders(); // for reset password token

            return services;
        }
    }
}
