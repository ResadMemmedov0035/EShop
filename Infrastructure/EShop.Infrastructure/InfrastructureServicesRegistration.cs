using EShop.Application.Mailing;
using EShop.Application.Security.Token;
using EShop.Application.Storages;
using EShop.Infrastructure.Mailing;
using EShop.Infrastructure.Security;
using EShop.Infrastructure.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services) 
        {
            services.AddScoped<ITokenHandler, JsonWebTokenHandler>();

            services.AddScoped<IMailService, MailService>();

            return services;
        }

        public static IServiceCollection AddStorage<TStorage>(this IServiceCollection services)
            where TStorage : Storage, IStorage
        {
            services.AddScoped<IStorage, TStorage>();
            services.AddScoped<IFormFileStorageService, FormFileStorageService>();

            return services;
        }
    }
}
