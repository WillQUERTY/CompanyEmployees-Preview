namespace PreviewApi.Application.Common;

/// <summary>
/// Interfaz base para Commands
/// </summary>
public interface ICommand<out TResponse>
{
}

/// <summary>
/// Interfaz base para Command Handlers
/// </summary>
public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interfaz base para Queries
/// </summary>
public interface IQuery<out TResponse>
{
}

/// <summary>
/// Interfaz base para Query Handlers
/// </summary>
public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default);
}

/// <summary>
/// Mediador para despachardispatcher Commands y Queries
/// </summary>
public interface IMediator
{
    Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
    Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
}
