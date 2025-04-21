using AuthenticatioApi.Application.Extensions;
using AuthenticatioApi.Core.Infrastructure.Configuration;
using AuthenticatioApi.Infra.Data.Extensions;
using AuthenticatioApi.Infra.Identity.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticatioApi.Infra.IoC.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddIoC(this IServiceCollection services, IConfiguration configuration, ApiConfig apiConfig)
        {
            services.AddAppServices();
            services.AddInfraData(configuration);
            services.AddIdentityIoC(configuration, apiConfig);
        }
    }
}
