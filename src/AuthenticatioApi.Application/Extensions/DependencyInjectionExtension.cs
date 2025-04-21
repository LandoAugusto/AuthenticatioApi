using AuthenticatioApi.Application.Interfaces;
using AuthenticatioApi.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticatioApi.Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddAppServices(this IServiceCollection services)
        {
                      
            services.AddScoped<IMenuScreenAppService, MenuScreenAppService>();
            services.AddScoped<IUserAppService, UserAppService>();
           
        }
    }
}
