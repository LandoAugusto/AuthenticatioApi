using Asp.Versioning;
using AuthenticatioApi.Configurations;
using AuthenticatioApi.Filters;
using AuthenticatioApi.Configurations;
using AuthenticatioApi.Core.Infrastructure;
using AuthenticatioApi.Core.Infrastructure.Configuration;
using AuthenticatioApi.Core.Infrastructure.Interfaces;
using AuthenticatioApi.Core.Infrastructure.Mapper;
using AuthenticatioApi.Infra.IoC.Extensions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO.Compression;
using System.Reflection;

namespace AuthenticatioApi.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void UseApi(this IServiceCollection services, IConfiguration configuration)
        {

            IConfigurationSection section = configuration.GetSection("ApiConfig");
            services.Configure<ApiConfig>(section);
            var apiConfig = section.Get<ApiConfig>();
            services.AddSingleton((IServiceProvider sp) => sp.GetRequiredService<IOptions<ApiConfig>>().Value);
            services.ConfigControllersPipeline();
            services.AddResponseCompression(apiConfig);
            services.AddUseCors();
            services.AddRoutings();
            services.AddApiVersion();
            services.AddSwagger(apiConfig);
            services.AddAutoMapper(typeof(ConfigurarationMapping));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddIoC(configuration, apiConfig);
        }

        private static IServiceCollection ConfigControllersPipeline(this IServiceCollection services)
        {
            services.AddScoped<IApiWorkContext, ApiWorkContext>();
            services.AddControllers(mvcOptions =>
            {

                //mvcOptions.Filters.Add<ContextFilter>(order: 1);
                //mvcOptions.Filters.Add<ControllersFilter>(order: 2);
                mvcOptions.Filters.Add<ExceptionFilter>(order: 0);
            });

            return services;
        }


        private static void AddResponseCompression(this IServiceCollection services, ApiConfig apiConfig)
        {
            if (apiConfig != null && apiConfig.UseResponseCompression)
            {
                services.Configure(delegate (GzipCompressionProviderOptions options)
                {
                    options.Level = CompressionLevel.SmallestSize;
                });

                services.AddResponseCompression(delegate (ResponseCompressionOptions options)
                {
                    options.EnableForHttps = true;
                    options.Providers.Add<BrotliCompressionProvider>();
                    options.Providers.Add<GzipCompressionProvider>();
                });
            }
        }

        private static void AddUseCors(this IServiceCollection services)
        {
            services.AddCors(delegate (CorsOptions options)
            {
                options.AddPolicy("AllowSpecificOrigins", delegate (CorsPolicyBuilder builder)
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        private static void AddRoutings(this IServiceCollection services)
        {
            services.AddRouting(delegate (RouteOptions option)
            {
                option.ConstraintMap["kebab"] = typeof(KebabParameterTransformer);
                option.LowercaseUrls = true;
            });
        }

        private static void AddApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(delegate (ApiVersioningOptions config)
            {
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                   new HeaderApiVersionReader("x-api-version"),
                                                   new MediaTypeApiVersionReader("x-api-version"));
            })
            .AddApiExplorer(options =>
            {   
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.DefaultApiVersionParameterDescription = "Versão da API.";
            }).EnableApiVersionBinding(); ;
        }

        private static void AddSwagger(this IServiceCollection services, ApiConfig apiConfig)
        {
            services.AddSwaggerGen(delegate (SwaggerGenOptions options)
            {
                options.OperationFilter<SwaggerDefaultValues>();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                    @"Digite 'Bearer' [espaço] e, em seguida, seu token na entrada de texto abaixo.<br><br>Example: 'Bearer 12345abcdef'<br>",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.OperationFilter<SwaggerJsonIgnoreFilter>();

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }
    }
}
