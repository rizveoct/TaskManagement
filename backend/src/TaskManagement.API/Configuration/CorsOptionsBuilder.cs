using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace TaskManagement.API.Configuration;

public static class CorsOptionsBuilder
{
    public const string DefaultPolicy = "DefaultCorsPolicy";

    public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? Array.Empty<string>();

        services.AddCors(options =>
        {
            options.AddPolicy(DefaultPolicy, policy =>
            {
                policy
                    .WithOrigins(allowedOrigins.Length == 0 ? new[] { "http://localhost:4200" } : allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}
