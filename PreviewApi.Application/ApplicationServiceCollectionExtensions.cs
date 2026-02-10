namespace PreviewApi.Application;

using PreviewApi.Application.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

/// <summary>
/// Extensiones para registrar servicios de aplicación
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registrar todos los Command y Query Handlers
        RegisterHandlers(services);

        // Registrar el mediador simple
        services.AddSingleton<IMediator, Mediator>();

        return services;
    }

    private static void RegisterHandlers(IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Registrar ICommandHandler<,> implementations
        var commandHandlerType = typeof(ICommandHandler<,>);
        var commandHandlers = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == commandHandlerType));

        foreach (var handler in commandHandlers)
        {
            var @interface = handler.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == commandHandlerType);
            services.AddScoped(@interface, handler);
        }

        // Registrar IQueryHandler<,> implementations
        var queryHandlerType = typeof(IQueryHandler<,>);
        var queryHandlers = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == queryHandlerType));

        foreach (var handler in queryHandlers)
        {
            var @interface = handler.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == queryHandlerType);
            services.AddScoped(@interface, handler);
        }
    }
}

public class Mediator : IMediator
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<Mediator> _logger;

    public Mediator(IServiceScopeFactory serviceScopeFactory, ILogger<Mediator> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        var handler = scope.ServiceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"No handler registered for command: {command.GetType().Name}");
        }

        _logger.LogInformation("Executing command: {Command}", command.GetType().Name);
        var method = handlerType.GetMethod("Handle");
        var result = await (Task<TResponse>)method!.Invoke(handler, new object[] { command, cancellationToken })!;

        return result;
    }

    public async Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
        var handler = scope.ServiceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"No handler registered for query: {query.GetType().Name}");
        }

        _logger.LogInformation("Executing query: {Query}", query.GetType().Name);
        var method = handlerType.GetMethod("Handle");
        var result = await (Task<TResponse>)method!.Invoke(handler, new object[] { query, cancellationToken })!;

        return result;
    }
}
