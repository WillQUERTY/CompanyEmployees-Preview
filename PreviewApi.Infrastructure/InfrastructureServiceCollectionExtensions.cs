namespace PreviewApi.Infrastructure;

using PreviewApi.Infrastructure.Data;
using PreviewApi.Infrastructure.Services;
using PreviewApi.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensiones para registrar servicios de infraestructura
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        // DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
                sqlOptions.EnableRetryOnFailure()));
        // Domain Services
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IResilienceService, ResilienceService>();

        // Security
        services.AddScoped<IJwtTokenService>(_ =>
            new JwtTokenService("your-secret-key-change-in-production-1234567890"));
        return services;
    }
}
