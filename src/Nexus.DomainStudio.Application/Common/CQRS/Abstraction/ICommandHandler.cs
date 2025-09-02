using Nexus.DomainStudio.Domain.Common;

namespace Nexus.DomainStudio.Application.CQRS.Abstraction;

/// <summary>
/// Interface for a command handler in the CQRS pattern.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface ICommandHandler<TCommand, TResponse> 
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
    /// <summary>
    /// Handles the specified command request and returns a response.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken);
}