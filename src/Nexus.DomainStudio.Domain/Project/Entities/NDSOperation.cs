using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Project.Enums;
using Nexus.DomainStudio.Domain.Project.Interfaces;
using Nexus.DomainStudio.Domain.Project.ValueObjects;

namespace Nexus.DomainStudio.Domain.Project.Entities;

/// <summary>
/// Represents an operation in Nexus Domain Studio.
/// </summary>
public sealed class NDSOperation : Entity<string>, INDSContextObject
{
    /// <summary>
    /// The name of the operation.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The description of the operation.
    /// </summary>
    public string? Description { get; private set; }
    
    // The arguments of the operation.
    private readonly List<NDSArgument> _arguments;

    // The emits of the operation.
    private readonly List<NDSOperationEmit> _emits;

    /// <summary>
    /// Creates a new NDSOperation instance with the specified name and optional description.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="arguments"></param>
    /// <param name="emits"></param>
    private NDSOperation(string id, string name, string? description, List<NDSArgument> arguments, List<NDSOperationEmit> emits) : base(id)
    {
        Name = name;
        Description = description;
        _arguments = arguments;
        _emits = emits;
    }

    /// <summary>
    /// The arguments of the operation.
    /// </summary>
    public IEnumerable<NDSArgument> Arguments => _arguments.AsReadOnly();

    /// <summary>
    /// The emits of the operation.
    /// </summary>
    public IEnumerable<NDSOperationEmit> Emits => _emits.AsReadOnly();

    /// <summary>
    /// Creates a new NDSOperation instance with the specified name, optional description, arguments, and emits.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="arguments"></param>
    /// <param name="emits"></param>
    /// <returns></returns>
    public static Result<NDSOperation> Create(string id, string name, string? description, List<NDSArgument> arguments, List<NDSOperationEmit> emits)
    {
        // Validate the name of the operation
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<NDSOperation>.Error("Operation name cannot be empty.");
        }

        // Create the NDSOperation instance
        var operation = new NDSOperation(id, name, description, arguments, emits);

        // Return the created operation
        return operation;
    }

    /// <summary>
    /// Gets the type of the context object.
    /// </summary>
    public NDSObjectType ObjectType => NDSObjectType.Operation;
}