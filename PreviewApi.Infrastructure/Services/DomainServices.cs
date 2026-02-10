namespace PreviewApi.Infrastructure.Services;

using Microsoft.Extensions.Logging;

/// <summary>
/// Servicio de auditoría para registrar cambios
/// </summary>
public interface IAuditService
{
    Task LogAsync(string entityType, string action, string userId, object? data);
}

public class AuditService : IAuditService
{
    private readonly ILogger<AuditService> _logger;

    public AuditService(ILogger<AuditService> logger)
    {
        _logger = logger;
    }

    public async Task LogAsync(string entityType, string action, string userId, object? data)
    {
        _logger.LogInformation(
            "AUDIT: Entity={Entity}, Action={Action}, User={User}, Data={Data}",
            entityType, action, userId, data);

        await Task.CompletedTask;
    }
}

/// <summary>
/// Servicio de notificaciones
/// </summary>
public interface INotificationService
{
    Task SendAsync(string subject, string message, string recipient);
}

public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger;
    }

    public async Task SendAsync(string subject, string message, string recipient)
    {
        _logger.LogInformation(
            "NOTIFICATION: To={Recipient}, Subject={Subject}",
            recipient, subject);

        // TODO: Implementar envío real (email, push, etc)
        await Task.CompletedTask;
    }
}

/// <summary>
/// Servicio de resiliencia - patrón Circuit Breaker simple
/// </summary>
public interface IResilienceService
{
    Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, int maxRetries = 3);
}

public class ResilienceService : IResilienceService
{
    private readonly ILogger<ResilienceService> _logger;

    public ResilienceService(ILogger<ResilienceService> logger)
    {
        _logger = logger;
    }

    public async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, int maxRetries = 3)
    {
        int attempt = 0;

        while (attempt < maxRetries)
        {
            try
            {
                _logger.LogInformation("Executing operation, Attempt {Attempt}/{MaxRetries}",
                    attempt + 1, maxRetries);

                return await operation();
            }
            catch (Exception ex) when (attempt < maxRetries - 1)
            {
                attempt++;
                _logger.LogWarning(ex,
                    "Operation failed, Attempt {Attempt}/{MaxRetries}. Retrying...",
                    attempt, maxRetries);

                await Task.Delay(1000 * attempt); // Exponential backoff
            }
        }

        throw new InvalidOperationException(
            $"Operation failed after {maxRetries} attempts");
    }
}
