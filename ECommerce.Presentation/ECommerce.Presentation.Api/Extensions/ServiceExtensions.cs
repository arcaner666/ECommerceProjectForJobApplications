using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.ConfigurationModels;
using ECommerce.Infrastructure.Logger.NLog;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Service.Contracts;
using ECommerce.Service.Services;
using ECommerce.Service.Services.Utilities.Security.Encryption;
using ECommerce.Service.Services.Utilities.Security.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Presentation.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCoreDependencies(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<RepositoryContext>();
    }

    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsConfiguration = configuration.GetSection("CorsConfiguration").Get<CorsConfiguration>();
        switch (corsConfiguration.Mode)
        {
            case "development":
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
                });
                break;
            case "production":
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", builder => builder
                        .WithOrigins(corsConfiguration.PermittedDomains.ToArray())
                        .WithMethods("GET", "OPTION", "POST", "PUT", "PATCH", "DELETE")
                        .AllowAnyHeader());
                });
                break;
        }
    }

    public static void ConfigureIISIntegration(this IServiceCollection services)
    {
        services.Configure<IISOptions>(options => { });
    }

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = tokenOptions.Issuer,
                ValidAudience = tokenOptions.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
            };
        });
    }

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static void ConfigurePatchRequests(this IServiceCollection services)
    {
        NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            return new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }

        services.AddControllers(config =>
        {
            config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
        });
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureServiceLayers(this IServiceCollection services)
    {
        services.AddScoped<IClaimDefaultService, ClaimDefaultService>();
        services.AddScoped<IClaimService, ClaimService>();

        services.AddScoped<IPersonAdvService, PersonAdvService>();
        services.AddScoped<IPersonCustomService, PersonCustomService>();
        services.AddScoped<IPersonService, PersonService>();

        services.AddScoped<IRoleDefaultService, RoleDefaultService>();
        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<IRoleClaimService, RoleClaimService>();

        services.AddScoped<IStartupService, StartupService>();
        services.AddScoped<IStartupRepository, StartupRepository>();

        services.AddScoped<ITokenHelper, JwtHelper>();
    }
}