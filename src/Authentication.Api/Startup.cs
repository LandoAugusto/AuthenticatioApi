using AuthenticatioApi.Extensions;
using Component.LogExtensions;

namespace AuthenticatioApi
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration = configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddLog(Configuration);
            services.UseApi(Configuration);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
            }

            app.UseLog();
            app.UseApi(Configuration);
        }
    }
}
