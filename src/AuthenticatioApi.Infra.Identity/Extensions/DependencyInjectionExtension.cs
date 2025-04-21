using AuthenticatioApi.Core.Infrastructure.Configuration;
using AuthenticatioApi.Infra.Identity.Configuration;
using AuthenticatioApi.Infra.Identity.Context;
using AuthenticatioApi.Infra.Identity.Interfaces;
using AuthenticatioApi.Infra.Identity.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthenticatioApi.Infra.Identity.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionExtension
    {
        private const string ConnectionName = "DefaultConnection";
        public static IServiceCollection AddIdentityIoC(this IServiceCollection services, IConfiguration configuration, ApiConfig apiConfig)
        {
            services
                .AddDbContext<IdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(ConnectionName)))
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()                
                .AddDefaultTokenProviders();

            services                
                .AddScoped<IUser,User>();

            services.AddAuthAndAuthor(apiConfig);

            return services;
        }


        private static void AddAuthAndAuthor(this IServiceCollection services, ApiConfig apiConfig)
        {
            if (apiConfig.Jwt.Enable)
            {
                var signingConfigurations = new SigningConfiguration();
                services.AddSingleton(signingConfigurations);



                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; ;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;                   
                }).AddJwtBearer(jwtOptions =>
                {
                    var secretKey = apiConfig.Jwt.Secret;
                    var validateSigningKey = !string.IsNullOrWhiteSpace(secretKey);

                    var paramsValidation = jwtOptions.TokenValidationParameters;

                    paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                    paramsValidation.ValidAudience = apiConfig.Jwt.Audience;
                    paramsValidation.ValidIssuer = apiConfig.Jwt.Issuer;
                    paramsValidation.ValidateIssuerSigningKey = true;
                    paramsValidation.ValidateLifetime = true;
                    paramsValidation.ClockSkew = TimeSpan.FromHours(apiConfig.Jwt.ExpiresInMinutes);                   
                    if (validateSigningKey)
                    {
                        var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
                        jwtOptions.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes);
                    }
                    else
                    {
                        jwtOptions.TokenValidationParameters.RequireExpirationTime = false;
                        jwtOptions.TokenValidationParameters.RequireSignedTokens = false;
                        jwtOptions.TokenValidationParameters.SignatureValidator = (token, _) =>
                            new JwtSecurityToken(token);
                    }

                    jwtOptions.TokenValidationParameters = paramsValidation;

                    jwtOptions.Events = new JwtBearerEvents
                    {
                        OnChallenge = async (context) =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        },
                        OnForbidden = async (context) =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        },
                    };
                });

                services.AddAuthorization();
            }
        }
    }
}
