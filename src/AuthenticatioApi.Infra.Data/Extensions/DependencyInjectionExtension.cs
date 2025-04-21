using AuthenticatioApi.Infra.Data.Contexts;
using AuthenticatioApi.Infra.Data.Interfaces;
using AuthenticatioApi.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticatioApi.Infra.Data.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfraData(this IServiceCollection services, IConfiguration configuration) =>
        services
        .AddDbContext<AuthenticatioDbContext>(
            options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped
        )
        .AddRepositories();

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMenuProductRepository, MenuProductRepository>();          

            return services;
        }
    }
}
