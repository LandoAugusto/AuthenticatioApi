using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using AuthenticatioApi.Core.Infrastructure.Configuration;
using AuthenticatioApi.Core.Infrastructure.Exceptions;
using AuthenticatioApi.Core.Model;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Net;

namespace AuthenticatioApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {

        const string SwaggerRoutePrefix = "api/swagger";
        public static void UseApi(this IApplicationBuilder application, IConfiguration configuration)
        {
            
            ApiConfig apiConfig = configuration.GetSection("ApiConfig").Get<ApiConfig>();
            application.UseCors("AllowSpecificOrigins");
            application.UseSwagger(apiConfig);
            application.UseApiExceptionHandler();
            application.UseResponseCompression();
            application.UseRoutingAndHttpsRedirection();
            application.UseAuthAndAuthor(apiConfig);
            application.UseEndpoint(apiConfig);

        }

        private static void UseApiExceptionHandler(this IApplicationBuilder application)
        {
            ILoggerFactory loggerFactory = application.ApplicationServices.GetRequiredService<ILoggerFactory>();
            application.UseExceptionHandler(delegate (IApplicationBuilder handler)
            {
                handler.Run(async delegate (HttpContext context)
                {
                    Exception ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                    HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

                    if (ex is NotFoundException) httpStatusCode = HttpStatusCode.NotFound;
                    if (ex is BusinessException) httpStatusCode = HttpStatusCode.BadRequest;
                    if (ex is UnauthorizedAccessException) httpStatusCode = HttpStatusCode.Unauthorized;
                    if (ex is ServiceUnavailableException) httpStatusCode = HttpStatusCode.ServiceUnavailable;
                    try
                    {
                        if (ex is ThreadAbortException)
                        {
                            return;
                        }
                        ;
                    }
                    finally
                    {
                        context.Response.StatusCode = (int)httpStatusCode;
                        context.Response.ContentType = "application/json";

                        var response = new BaseDataResponseModel<object>
                        {
                            TransactionStatus = new StatusResponseModel
                            {
                                Code = (int)httpStatusCode,
                                Message = ((!string.IsNullOrEmpty(ex.Message)) ? ex.Message : "Sistema temporariamente indisponível."),
                                Details = $"{"InnerException"}: {ex.InnerException} ---- {"StackTrace"}: {ex.StackTrace}"
                            }
                        };

                        loggerFactory.CreateLogger($"catch-all-{httpStatusCode}").LogError(ex, ex.Message);

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                    }
                });
            });
        }
        private static void UseRoutingAndHttpsRedirection(this IApplicationBuilder application)
        {
            application.UseHttpsRedirection();
            application.UseRouting();
        }

        private static void UseResponseCompression(this IApplicationBuilder application)
        {
            //if (commonConfig.UseResponseCompression)
            //{
            //    application.UseResponseCompression();
            //}
        }

        private static void UseSwagger(this IApplicationBuilder application, ApiConfig commonConfig)
        {
            IApiVersionDescriptionProvider provider = application.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            application.UseSwagger(delegate (SwaggerOptions options)
            {
                options.RouteTemplate = $"{SwaggerRoutePrefix}/{{documentName}}/swagger.json";
            });
            application.UseSwaggerUI(delegate (SwaggerUIOptions options)
            {
                options.RoutePrefix = SwaggerRoutePrefix;
                foreach (ApiVersionDescription apiVersionDescription in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/{SwaggerRoutePrefix}/{apiVersionDescription.GroupName}/swagger.json", $"{commonConfig.ApplicationName}-{apiVersionDescription.GroupName.ToUpperInvariant()}");
                    options.DisplayRequestDuration();
                }
            });
        }

        private static void UseAuthAndAuthor(this IApplicationBuilder application, ApiConfig apiConfig)
        {
            if (apiConfig.Jwt.Enable)
            {
                application.UseAuthentication();
                application.UseAuthorization();
            }

            application.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });
        }

        private static void UseEndpoint(this IApplicationBuilder application, ApiConfig apiConfig)
        {
            application.UseEndpoints(delegate (IEndpointRouteBuilder endpoints)
            {
                endpoints.MapControllers();
            });
        }
    }
}
