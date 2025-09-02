namespace Nexus.DomainStudio.Application.CQRS.Abstraction;

/// <summary>
/// Interface for a command in the CQRS pattern without a response type.
/// </summary>
// public interface IQuery { }

/// <summary>
/// Interface for a command in the CQRS pattern.
/// </summary>
/// <typeparam name="TResponse">The response type expected.</typeparam>
public interface IQuery<TResponse> { }
